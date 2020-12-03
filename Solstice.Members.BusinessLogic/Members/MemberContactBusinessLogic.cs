using Microsoft.EntityFrameworkCore;
using Solstice.Core.Models;
using Solstice.DAL.Marketplace;
using Solstice.Members.MemberContactModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.Members
{
    sealed class MemberContactBusinessLogic : IMemberContactBusinessLogic
    {
        private IMarketplaceDbContext Db { get; }

        public MemberContactBusinessLogic(IMarketplaceDbContext marketplaceDbContext)
        {
            Db = marketplaceDbContext;
        }

        public async Task<MemberContactInfo> GetMemberContactAsync(int personId)
        {
            var query =
                from m in Db.Members_vw_Member
                where m.PersonID == personId

                join ph in Db.Members_vw_MemberPhone on new { m.PersonID, IsCurrent = true } equals new { ph.PersonID, ph.IsCurrent } into phItems
                from ph in phItems.DefaultIfEmpty()

                select new
                {
                    m.EmailAddress,

                    PrimaryPhoneNumber = new PhoneNumber
                    {
                        Number = ph.PhoneNumber,
                        Extension = ph.PhoneExtension
                    }
                };

            var queryResult = await query.SingleOrDefaultAsync();

            var result = new MemberContactInfo
            {
                PersonId = personId
            };

            if (queryResult != null)
            {
                result.EmailAddress = queryResult.EmailAddress;
                result.PrimaryPhoneNumber = queryResult.PrimaryPhoneNumber;
            }

            return result;
        }

        public async Task UpdateMemberEmailAsync(
            int personId,
            string emailAddress
        )
        {
            emailAddress = emailAddress.Sanitize();

            if (!emailAddress.HasValue())
            {
                throw new ArgumentException(nameof(emailAddress));
            }

            if (!emailAddress.IsValidEmailAddress())
            {
                throw new ArgumentException(nameof(emailAddress));
            }

            using (var transaction = TransactionScopeHelper.CreateForAsync())
            {
                var person = (
                        from p in Db.Person
                        where p.PersonID == personId
                        select p
                    ).Single();

                person.Email = emailAddress;

                await Db.SaveChangesAsync();

                transaction.Complete();
            }
        }
    }
}
