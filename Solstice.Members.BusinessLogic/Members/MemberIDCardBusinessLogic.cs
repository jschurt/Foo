using Microsoft.EntityFrameworkCore;
using Solstice.Core.Models;
using Solstice.DAL.Marketplace;
using Solstice.Members.MemberIdCardModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.Members
{
    sealed class MemberIdCardBusinessLogic : IMemberIdCardBusinessLogic
    {
        private IMarketplaceDbContext Db { get; }
        private IBenefitBusinessLogic BenefitBusinessLogic { get; }

        private class MemberIdCardInfo
        {
            public string DisplayMemberNumber { get; set; }
            public string MemberName { get; set; }
            public string MemberCoveredDependents { get; set; }
            public string GroupName { get; set; }
            public bool IsIndividual { get; set; }
            public string BenefitEffectiveDate { get; set; }
            public string ProductNames { get; set; }
        }

        public MemberIdCardBusinessLogic(
            IMarketplaceDbContext marketplaceDbContext,
            IBenefitBusinessLogic benefitBusinessLogic
            )
        {
            Db = marketplaceDbContext;
            BenefitBusinessLogic = benefitBusinessLogic;
        }

        public async Task<GetMemberIdCardResult> GetMemberIdCardAsync(int personId, IdCardFormat format)
        {
            if (personId <= 0)
            {
                return null;
            }

            switch (format)
            {
                case IdCardFormat.Png:
                    break;
                default:
                    throw new ArgumentException(nameof(format));
            }

            var benefits = await GetBenefitsEligibleForIdCardAsync(personId);

            if (!benefits.Any())
            {
                return null;
            }

            var subscriberPersonId = (int)await Db.Members_vw_Member
                .Where(i => i.PersonID == personId)
                .Select(i => i.ParentPersonID ?? i.PersonID)
                .SingleAsync();

            var subscriberInfo = await (
                from m in Db.Members_vw_Member
                join g in Db.Members_vw_MemberGroup on m.PersonID equals g.PersonID
                where m.PersonID == subscriberPersonId
                select new
                {
                    m.DisplayMemberNumber,
                    g.GroupName,
                    g.IsIndividual
                }
                )
                .FirstAsync();

            var members = (
                from b in benefits
                from m in b.Members
                select new
                {
                    m.IsSubscriber,
                    m.PersonId,
                    m.LastName,
                    m.FirstName
                })
                .Distinct()
                .OrderBy(i => i.IsSubscriber)
                .ThenBy(i => i.LastName)
                .ThenBy(i => i.FirstName)
                .Select(i => new
                {
                    i.IsSubscriber,
                    FullName = $"{i.FirstName} {i.LastName}"
                })
                .ToList();

            var benefitEffectiveDate = benefits.Select(i => i.StartDate).Min();

            var productNames =
                from b in benefits
                select $"{b.Plan.Product.Name}: {b.Plan.Name}";

            var subscriberName = members
                .Where(i => i.IsSubscriber == true)
                .Select(i => i.FullName)
                .Single();

            var dependentNames = members
                .Where(i => i.IsSubscriber == false)
                .Select(i => i.FullName);

            var memberIdCardInfo = new MemberIdCardInfo
            {
                DisplayMemberNumber = subscriberInfo.DisplayMemberNumber,
                MemberName = subscriberName,
                GroupName = subscriberInfo.GroupName,
                IsIndividual = subscriberInfo.IsIndividual,
                BenefitEffectiveDate = benefitEffectiveDate.ToString("MM/dd/yyyy"),
                ProductNames = string.Join(" | ", productNames.Distinct()),
                MemberCoveredDependents = string.Join(", ", dependentNames)
            };

            var result = new GetMemberIdCardResult
            {
                Format = format
            };

            switch (format)
            {
                case IdCardFormat.Png:
                    result.Content = GenerateIDCard(memberIdCardInfo);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        private byte[] GenerateIDCard(MemberIdCardInfo memberIdCardInfo)
        {
            const float xCoord = 360f;
            const float labelXcoord = 39f;

            Stream streamImage;

            if (memberIdCardInfo.IsIndividual)
                streamImage = new MemoryStream(MemberIdCardResources.IDCard);
            else
                streamImage = new MemoryStream(MemberIdCardResources.IDCardWithGroup);

            using (streamImage)
            {
                using (var bitmap = (Bitmap)Image.FromStream(streamImage))
                {
                    float width = bitmap.Size.Width;

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                        using (Font font = new Font("Montserrat", 25))
                        {
                            SizeF sizeFirst = graphics.MeasureString(memberIdCardInfo.MemberName, font);
                            SizeF sizeSec = graphics.MeasureString(memberIdCardInfo.DisplayMemberNumber, font);

                            PointF firstLocation = new PointF((width - sizeFirst.Width) / 2, 210f);
                            PointF secondLocation = new PointF((width - sizeSec.Width) / 2, 245f);

                            graphics.DrawString(memberIdCardInfo.MemberName, font, Brushes.Black, firstLocation);
                            graphics.DrawString(memberIdCardInfo.DisplayMemberNumber, font, Brushes.Black, secondLocation);
                        }

                        using (Font font = new Font("Montserrat", 20))
                        {
                            if (memberIdCardInfo.IsIndividual == false)
                            {
                                PointF groupNameLabelLocation = new PointF(labelXcoord, 366f);
                                PointF thirdLocation = new PointF(xCoord, 366f);
                                graphics.DrawString("Group Name:", font, Brushes.Black, groupNameLabelLocation);
                                graphics.DrawString(memberIdCardInfo.GroupName, font, Brushes.Black, thirdLocation);
                            }

                            PointF EffectiveDateLabelLocation = new PointF(labelXcoord, 427f);
                            PointF fourthLocation = new PointF(xCoord, 427f);
                            PointF productLabelLocation = new PointF(labelXcoord, 488f);
                            PointF fifthLocation = new PointF(xCoord, 488f);
                            PointF coveredDependentsLabelLocation = new PointF(labelXcoord, 547f);
                            PointF sixthLocation = new PointF(xCoord, 547f);

                            graphics.DrawString("Effective Date:", font, Brushes.Black, EffectiveDateLabelLocation);
                            graphics.DrawString(memberIdCardInfo.BenefitEffectiveDate, font, Brushes.Black, fourthLocation);
                            graphics.DrawString("Product(s):", font, Brushes.Black, productLabelLocation);
                            graphics.DrawString(memberIdCardInfo.ProductNames, font, Brushes.Black, fifthLocation);
                            graphics.DrawString("Covered Dependents:", font, Brushes.Black, coveredDependentsLabelLocation);
                            graphics.DrawString(memberIdCardInfo.MemberCoveredDependents, font, Brushes.Black, sixthLocation);
                        }

                        byte[] byteArray;

                        using (var stream = new MemoryStream())
                        {
                            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            stream.Flush();

                            byteArray = stream.ToArray();
                        }

                        return byteArray;
                    }
                }
            }
        }

        public async Task<bool> IsMemberIdCardEligibleAsync(int personId)
        {
            if (personId <= 0)
            {
                return false;
            }

            var benefits = await GetBenefitsEligibleForIdCardAsync(personId);

            return benefits.Any();
        }

        private async Task<List<BenefitModels.BenefitInfo>> GetBenefitsEligibleForIdCardAsync(int personId)
        {
            return (
               from i in await BenefitBusinessLogic.GetCurrentBenefitsAsync(personId, true, excludePremium: true)
               where
                   i.Plan.Product.ProductId == ProductID.Dental ||
                   i.Plan.Product.ProductId == ProductID.Vision
               orderby
                   i.Plan.Product.Name
               select i
               )
               .ToList();
        }
    }
}
