using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Solstice.Core.Models;
using Solstice.DAL.Marketplace;
using Solstice.DAL.Workbench;
using Solstice.Members.BenefitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solstice.Members
{
    internal sealed class BenefitBusinessLogic : IBenefitBusinessLogic
    {
        private readonly IMarketplaceDbContext _marketplaceDbContext;
        private readonly BenefitBusinessLogicSettings _settings;
        private readonly IWorkbenchDbContext _workbenchDbContext;

        public BenefitBusinessLogic(
            IMarketplaceDbContext marketplaceDbContext,
            IOptions<BenefitBusinessLogicSettings> settings,
            IWorkbenchDbContext workbenchDbContext
            )
        {
            _marketplaceDbContext = marketplaceDbContext;
            _settings = settings.Value;
            _workbenchDbContext = workbenchDbContext;
        }

        private IQueryable<DAL.Marketplace.Models.Members_udf_MemberBenefitAggregated> GetMemberBenefitAggregated(
            int personId,
            bool includeFamily,
            bool currentOnly,
            bool excludePremium
            )
        {
            var baseQuery = _marketplaceDbContext.Members_udf_MemberBenefitAggregated(personId, includeFamily: includeFamily, excludePremium: excludePremium);

            if (currentOnly)
            {
                var today = DateTime.Today;

                baseQuery =
                    from i in baseQuery
                    where
                        i.BenefitEffectiveDate <= today &&
                        i.BenefitExpireDate >= today
                    select i;
            }

            return baseQuery;
        }

        private async Task<ILookup<int, string>> GetRiderPlanNumbersForPlansAsync(IEnumerable<int> planIds)
        {
            // Can't use async enumerable (on .NET Core 2.2).
            // Have to use subqueries instead of two left joins due to lack of support in EF Core 2.2 (it was resolving in memory).
            var riderPlanNumbersBase = await (
                from pl in _marketplaceDbContext.vw_Plan
                where
                    // TODO: Implement efficient parameterized query. Needs support from Solstice.DAL.Core.
                    planIds.Contains((int)pl.PlanID) &&
                    (
                        pl.RiderDentalPlanID > 0 ||
                        pl.RiderVisionPlanID > 0
                    )
                select new
                {
                    PlanID = (int)pl.PlanID,
                    RiderDentalPlanNumber = (
                        from pld in _marketplaceDbContext.vw_Plan
                        where pld.PlanID == pl.RiderDentalPlanID
                        select pld.PlanNumber
                        )
                        .FirstOrDefault(),
                    RiderVisionPlanNumber = (
                        from plv in _marketplaceDbContext.vw_Plan
                        where plv.PlanID == pl.RiderVisionPlanID
                        select plv.PlanNumber
                        )
                        .FirstOrDefault()
                })
                .ToListAsync();

            var dentalRiderPlanNumbers = riderPlanNumbersBase
                .Where(i => i.RiderDentalPlanNumber.HasValue())
                .Select(i => new
                {
                    i.PlanID,
                    PlanNumber = i.RiderDentalPlanNumber
                });

            var visionRiderPlanNumbers = riderPlanNumbersBase
                .Where(i => i.RiderVisionPlanNumber.HasValue())
                .Select(i => new
                {
                    i.PlanID,
                    PlanNumber = i.RiderVisionPlanNumber
                });

            var result = dentalRiderPlanNumbers
                .Concat(visionRiderPlanNumbers)
                .Distinct()
                .ToLookup(k => k.PlanID, v => v.PlanNumber);

            return result;
        }

        private async Task<List<BenefitInfo>> GetBenefitsAsync(int personId, bool includeFamily, bool currentOnly, bool excludePremium)
        {
            var baseQuery = GetMemberBenefitAggregated(personId, includeFamily, currentOnly, excludePremium);

            var query =
                from b in baseQuery
                join m in _marketplaceDbContext.Members_vw_Member on b.PersonID equals m.PersonID
                join pl in _marketplaceDbContext.Plans_vw_Plan on b.PlanID equals pl.PlanID
                select new
                {
                    PersonID = (int)b.PersonID,
                    MemberFirstName = m.FirstName,
                    MemberMiddleName = m.MiddleName,
                    MemberLastName = m.LastName,
                    MemberIsSubscriber = m.IsSubscriber,

                    pl.PlanProductTypeID,
                    pl.PlanProductTypeName,

                    PlanID = (int)pl.PlanID,
                    pl.PlanName,
                    pl.PlanNumber,

                    pl.PlanTypeID,
                    pl.PlanTypeName,

                    b.BenefitEffectiveDate,
                    b.BenefitExpireDate,

                    b.MonthlyPremiumFamily
                };

            var queryResults = await query.ToListAsync();

            var planIds = queryResults
                .Select(i => i.PlanID)
                .Distinct();

            var riderPlanNumbersByPlanId = await GetRiderPlanNumbersForPlansAsync(planIds);

            var groupedResults =
                from i in queryResults
                let k = new
                {
                    i.PlanProductTypeID,
                    i.PlanProductTypeName,
                    i.PlanID,
                    i.PlanName,
                    i.PlanNumber,
                    i.PlanTypeID,
                    i.PlanTypeName,
                    i.BenefitEffectiveDate,
                    i.BenefitExpireDate,
                    i.MonthlyPremiumFamily
                }
                group i by k into g
                let i = g.Key
                orderby
                    i.BenefitEffectiveDate,
                    i.PlanID
                select new BenefitInfo
                {
                    Plan = new PlanInfo
                    {
                        PlanId = i.PlanID,
                        Name = i.PlanName,
                        PlanNumber = i.PlanNumber,

                        Product = new ProductInfo
                        {
                            ProductId = (ProductID)i.PlanProductTypeID,
                            Name = i.PlanProductTypeName,
                        },

                        PlanType = new PlanTypeInfo
                        {
                            PlanTypeId = (PlanTypeID)i.PlanTypeID,
                            Name = i.PlanTypeName
                        },

                        RiderPlanNumbers = riderPlanNumbersByPlanId[i.PlanID].ToList()
                    },
                    StartDate = i.BenefitEffectiveDate,
                    EndDate = i.BenefitExpireDate,
                    MonthlyPremium = i.MonthlyPremiumFamily,
                    Members = (
                        from m in g
                        select new MemberInfo
                        {
                            PersonId = m.PersonID,
                            FirstName = m.MemberFirstName,
                            MiddleName = m.MemberMiddleName,
                            LastName = m.MemberLastName,
                            IsSubscriber = m.MemberIsSubscriber
                        })
                        .ToArray()
                };

            var results = groupedResults.ToList();

            return results;
        }

        public Task<List<BenefitInfo>> GetAllBenefitsAsync(int personId, bool includeFamily, bool excludePremium = false)
        {
            return GetBenefitsAsync(personId, includeFamily, currentOnly: false, excludePremium: excludePremium);
        }

        public Task<List<BenefitInfo>> GetCurrentBenefitsAsync(int personId, bool includeFamily, bool excludePremium = false)
        {
            return GetBenefitsAsync(personId, includeFamily, currentOnly: true, excludePremium: excludePremium);
        }

        private async Task<List<ProductInfo>> GetProductsAsync(int personId, bool includeFamily, bool currentOnly)
        {
            var productIds = GetMemberBenefitAggregated(personId, includeFamily, currentOnly, true)
                .Select(i => i.PlanProductTypeID)
                .Distinct();

            var query =
                from i in productIds
                join p in _marketplaceDbContext.Plans_vw_PlanProductType on i equals p.PlanProductTypeID
                select new ProductInfo
                {
                    ProductId = (ProductID)p.PlanProductTypeID,
                    Name = p.PlanProductTypeName
                };

            var result = await query.ToListAsync();

            return result;
        }

        public Task<List<ProductInfo>> GetAllProductsAsync(int personId, bool includeFamily)
        {
            return GetProductsAsync(personId, includeFamily, false);
        }

        public Task<List<ProductInfo>> GetCurrentProductsAsync(int personId, bool includeFamily)
        {
            return GetProductsAsync(personId, includeFamily, true);
        }

        private async Task<List<PlanInfo>> GetPlansAsync(int personId, bool includeFamily, bool currentOnly)
        {
            var planIds = GetMemberBenefitAggregated(personId, includeFamily, currentOnly, true)
                .Where(i => i.PlanID != null)
                .Select(i => i.PlanID)
                .Distinct();

            var query =
                from i in planIds
                join p in _marketplaceDbContext.Plans_vw_Plan on i equals p.PlanID
                select new PlanInfo
                {
                    PlanId = (int)p.PlanID,
                    Name = p.PlanName,
                    PlanNumber = p.PlanNumber,

                    Product = new ProductInfo
                    {
                        ProductId = (ProductID)p.PlanProductTypeID,
                        Name = p.PlanProductTypeName,
                    },

                    PlanType = new PlanTypeInfo
                    {
                        PlanTypeId = (PlanTypeID)p.PlanTypeID,
                        Name = p.PlanTypeName
                    }
                };

            var result = await query.ToListAsync();

            return result;
        }

        public Task<List<PlanInfo>> GetAllPlansAsync(int personId, bool includeFamily)
        {
            return GetPlansAsync(personId, includeFamily, false);
        }

        public Task<List<PlanInfo>> GetCurrentPlansAsync(int personId, bool includeFamily)
        {
            return GetPlansAsync(personId, includeFamily, true);
        }

        public async Task<List<ScheduleOfBenefitsInfo>> GetCurrentScheduleOfBenefitsAsync(int personId)
        {
            var query =
                from i in GetMemberBenefitAggregated(personId, false, true, true)
                where i.PlanID != null
                select new
                {
                    PlanID = (int)i.PlanID,
                    i.ScheduleOfBenefitsXml
                };

            var queryResult = await query.ToListAsync();

            var result = (
                from i in queryResult.Distinct()
                where i.ScheduleOfBenefitsXml.HasValue()
                from i2 in XElement.Parse(i.ScheduleOfBenefitsXml).Elements()
                select new ScheduleOfBenefitsInfo
                {
                    PlanId = i.PlanID,
                    DisplayName = (string)i2.Attribute("DisplayName"),
                    DocumentUrl = _settings.ScheduleOfBenefitsDocumentBaseUrl + (string)i2.Attribute("FileName"),
                    Language = (ScheduleOfBenefitsInfo.LanguageID)(int)i2.Attribute("LanguageID")
                }
                )
                .ToList();

            return result;
        }

        public async Task<List<ValueAddonInfo>> GetCurrentValueAddonDocumentsAsync(int personId)
        {
            var query =
                from i in GetMemberBenefitAggregated(personId, false, true, true)
                where i.PlanID != null
                select new
                {
                    PlanID = (int)i.PlanID,
                    i.ValueAddonXml
                };

            var queryResult = await query.ToListAsync();

            var result = (
                from i in queryResult.Distinct()
                where i.ValueAddonXml.HasValue()
                from i2 in XElement.Parse(i.ValueAddonXml).Elements()
                select new ValueAddonInfo
                {
                    PlanID = i.PlanID,
                    DisplayName = (string)i2.Attribute("DisplayName"),
                    Name = (string)i2.Attribute("Name"),
                    Path = (string)i2.Attribute("Path"),
                    DocumentUrl = _settings.ValueAddonsBaseUrl + (string)i2.Attribute("Name"),
                    ValueAddonDescription = (string)i2.Attribute("ValueAddonDescription"),
                    DocumentID = (int)i2.Attribute("DocumentID")
                }
                )
                .ToList();

            return result;
        }

        public async Task<bool> HasCurrentDavisVisionPlan(int personId)
        {
            var planNumbers =
                from i in await GetCurrentPlansAsync(personId, false)
                where i.Product.ProductId == ProductID.Vision
                select i.PlanNumber;

            foreach (var planNumber in planNumbers)
            {
                var hasDavisVisionQuery = await (
                    from i in _workbenchDbContext.Plans_vw_PlanInsuranceCompanyGroup
                    where
                        i.InsuranceCompanyGroupName == "DavisVision" &&
                        i.PlanNumber == planNumber
                    select i
                    )
                    .AnyAsync();

                if (hasDavisVisionQuery)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
