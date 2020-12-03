using Microsoft.Data.SqlClient;
using Solstice.DAL;
using Solstice.DAL.Marketplace;
using Solstice.DAL.Marketplace.Models;
using Solstice.Members.EnrollmentModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Solstice.Members
{
    sealed class EnrollmentBusinessLogic : IEnrollmentBusinessLogic
    {
        /// <summary>
        /// Oldest benefit end date allowed.
        /// </summary>
        private static readonly DateTime MaxBenefitEndDate = new DateTime(2999, 12, 31);

        private readonly IMarketplaceDbContext _marketplaceDbContext;
        private readonly ISqlUtility<IMarketplaceDbContext> _sqlUtility;

        public EnrollmentBusinessLogic(IMarketplaceDbContext marketplaceDbContext, ISqlUtility<IMarketplaceDbContext> sqlUtility)
        {
            _marketplaceDbContext = marketplaceDbContext;
            _sqlUtility = sqlUtility;
        }

        public async Task<int> GetOrCreateBillingPeriodAsync(decimal groupID, int planID, DateTime benefitStartDate, ProcessFileMode mode)
        {
            var billingPeriodQuery =
                from bp in _marketplaceDbContext.BillingPeriod
                select new
                {
                    bp.BillingPeriodID,
                    bp.StartDate,
                    bp.EndDate
                };

            var planInfo = await (
                from pl in _marketplaceDbContext.Plan
                join bp in billingPeriodQuery on pl.BillingPeriodID equals bp.BillingPeriodID into bpItems
                from bp in bpItems.DefaultIfEmpty()
                where pl.PlanID == planID
                select new
                {
                    PlanName = pl.Name,
                    BillingPeriod = bp
                }
                )
                .SingleAsync();

            var benefitPeriodId = 0;

            #region Gets or creates the billing period

            var benefitPeriodBaseQuery =
                from bp in _marketplaceDbContext.GroupProductBenefitPeriod.AllActiveLegacy()
                where
                    bp.GroupID == groupID &&
                    bp.PlanProductTypeID == null &&
                    bp.PlanTypeID == null &&
                    bp.PlanID == null &&
                    bp.ParentBenefitPeriodID == null
                select bp;

            if (planInfo.BillingPeriod != null)
            {
                benefitPeriodId = await (
                    from bp in benefitPeriodBaseQuery
                    where
                        bp.BenefitEffectiveDate <= planInfo.BillingPeriod.StartDate &&
                        bp.BenefitExpireDate >= planInfo.BillingPeriod.StartDate
                    select (int)bp.GroupProductBenefitPeriodID
                    )
                    .SingleOrDefaultAsync();

                if (benefitPeriodId <= 0)
                {
                    var benefitPeriod = new GroupProductBenefitPeriod
                    {
                        GroupID = groupID,
                        BenefitEffectiveDate = planInfo.BillingPeriod.StartDate,
                        BenefitExpireDate = planInfo.BillingPeriod.EndDate
                    };

                    await _marketplaceDbContext.GroupProductBenefitPeriod.AddAsync(benefitPeriod);

                    await _marketplaceDbContext.SaveChangesAsync();

                    benefitPeriodId = (int)benefitPeriod.GroupProductBenefitPeriodID;
                }
            }
            else
            {
                benefitPeriodId = await (
                    from bp in benefitPeriodBaseQuery
                    where
                        bp.BenefitEffectiveDate <= benefitStartDate &&
                        bp.BenefitExpireDate >= benefitStartDate
                    select (int)bp.GroupProductBenefitPeriodID
                    )
                    .SingleOrDefaultAsync();

                if (benefitPeriodId <= 0)
                {
                    var startDate = new DateTime(benefitStartDate.Year, 1, 1);

                    var benefitPeriod = new GroupProductBenefitPeriod
                    {
                        GroupID = groupID,
                        BenefitEffectiveDate = startDate
                    };

                    switch (mode)
                    {
                        case ProcessFileMode.EssentialSmileIndividual:
                            benefitPeriod.BenefitExpireDate = startDate.AddYears(1).AddDays(-1);
                            break;
                        case ProcessFileMode.SolsticeInsuranceIndividual:
                            benefitPeriod.BenefitExpireDate = MaxBenefitEndDate;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    await _marketplaceDbContext.GroupProductBenefitPeriod.AddAsync(benefitPeriod);

                    await _marketplaceDbContext.SaveChangesAsync();

                    benefitPeriodId = (int)benefitPeriod.GroupProductBenefitPeriodID;
                }
            }

            #endregion

            #region Creates the GroupPlan if required

            var groupStruture = await _marketplaceDbContext.GroupStructure.SingleAsync(a => a.GroupID == groupID);

            var mustCreateGroupPlan = (
                from gp in _marketplaceDbContext.GroupPlan.AllActiveLegacy()
                where
                    gp.GroupID == groupID &&
                    gp.GroupStructureID == groupStruture.GroupStructureID &&
                    gp.PlanID == planID &&
                    gp.GroupProductBenefitPeriodID == benefitPeriodId
                select gp
                )
                .Any() == false;

            if (mustCreateGroupPlan)
            {
                var groupPlan = new GroupPlan
                {
                    GroupID = groupID,
                    GroupStructureID = groupStruture.GroupStructureID,
                    PlanID = planID,
                    Name = planInfo.PlanName,
                    GroupProductBenefitPeriodID = benefitPeriodId
                };

                _marketplaceDbContext.GroupPlan.Add(groupPlan);

                _marketplaceDbContext.SaveChanges();
            }

            #endregion
            return benefitPeriodId;
        }

        public async Task<int> CreateEventAsync(
            int benefitPeriodId,
            int groupEmployeeId,
            LookupAttributes.EventType type,
            LookupAttributes.EventUpdateMethodType updateMethod,
            LookupAttributes.QualifyingLifeEventReason? reason = null,
            DateTime? eventDate = null,
            DateTime? userDueDate = null,
            DateTime? changeEffectiveDate = null,
            bool setAsCompleted = false)
        {
            if (groupEmployeeId <= 0)
            {
                throw new ArgumentException(nameof(groupEmployeeId));
            }

            if (!type.IsValueDefined<LookupAttributes.EventType>())
            {
                throw new ArgumentException(nameof(type));
            }

            if (benefitPeriodId <= 0)
            {
                throw new ArgumentException(nameof(benefitPeriodId));
            }

            if (!updateMethod.IsValueDefined<LookupAttributes.EventUpdateMethodType>())
            {
                throw new ArgumentException(nameof(updateMethod));
            }

            if (reason.HasValue && !reason.IsValueDefined<LookupAttributes.QualifyingLifeEventReason>())
            {
                throw new ArgumentException(nameof(reason));
            }

            if (eventDate != null)
            {
                eventDate = eventDate.Value.Date;
            }

            if (userDueDate != null)
            {
                userDueDate = userDueDate.Value.Date;
            }

            if (changeEffectiveDate != null)
            {
                changeEffectiveDate = changeEffectiveDate.Value.Date;
            }

            var eventIdParam = new SqlParameter("@EventID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            using (var tran = TransactionScopeHelper.CreateForAsync())
            {
                await _sqlUtility.ExecuteStoredProcedureAsync(
                    "dbo.usp_Membership_CreateEvent",
                    new SqlParameter("@BenefitPeriodID", SqlDbType.Int) { Value = benefitPeriodId },
                    new SqlParameter("@GroupEmployeeID", SqlDbType.Int) { Value = groupEmployeeId },
                    new SqlParameter("@Type", SqlDbType.Int) { Value = (int)type },
                    new SqlParameter("@UpdateMethod", SqlDbType.Int) { Value = (int)updateMethod },
                    new SqlParameter("@Reason", SqlDbType.Int) { Value = (int?)reason },
                    new SqlParameter("@EventDate", SqlDbType.Date) { Value = eventDate },
                    new SqlParameter("@UserDueDate", SqlDbType.Date) { Value = userDueDate },
                    new SqlParameter("@ChangeEffectiveDate", SqlDbType.Date) { Value = changeEffectiveDate },
                    new SqlParameter("@SetAsCompleted", SqlDbType.Bit) { Value = setAsCompleted },
                    eventIdParam
                    );

                tran.Complete();
            }

            return (int)eventIdParam.Value;
        }

        public async Task CompleteEventAsync(int eventId, bool process = false)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException(nameof(eventId));
            }

            using (var tran = TransactionScopeHelper.CreateForAsync())
            {
                await _sqlUtility.ExecuteStoredProcedureAsync(
                    "dbo.usp_Membership_CompleteEvent",
                    new SqlParameter("@EventID", SqlDbType.Int) { Value = eventId }
                    );

                await UpdatePremiumAsync(eventId);

                if (process)
                {
                    await ProcessEventAsync(eventId);
                }

                tran.Complete();
            }
        }

        private async Task ProcessEventAsync(int eventId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException(nameof(eventId));
            }

            TransactionScopeHelper.EnsureAmbientTransaction();

            await _sqlUtility.ExecuteStoredProcedureAsync(
                "dbo.usp_Membership_ProcessEvent",
                new SqlParameter("@EventID", System.Data.SqlDbType.Int) { Value = eventId }
                );
        }

        private async Task UpdatePremiumAsync(int eventId, bool force = false)
        {
            using (var tran = TransactionScopeHelper.CreateForAsync())
            {
                await _sqlUtility.ExecuteStoredProcedureAsync(
                    "dbo.usp_Membership_UpdatePremium",
                    new SqlParameter("@EventID", SqlDbType.Int) { Value = eventId },
                    new SqlParameter("@Force", SqlDbType.Bit) { Value = force }
                );

                tran.Complete();
            }
        }

        public async Task AddCoverageAsync(int eventId, int personId, int planId, DateTime benefitStartDate, DateTime benefitEndDate)
        {
            #region Add Coverage
            {
                // var healthcareProviderInfo = GetHealthcareProviderInfoFromRow(row);

                await _sqlUtility.ExecuteStoredProcedureAsync(
                    "dbo.usp_EligibilityUploadIndividualControl_ProcessAdd",
                    new SqlParameter("@EventID", SqlDbType.Int) { Value = eventId },
                    new SqlParameter("@PersonID", SqlDbType.Int) { Value = personId },
                    new SqlParameter("@PlanID", SqlDbType.Int) { Value = planId },
                    new SqlParameter("@StartDate", SqlDbType.Date) { Value = benefitStartDate },
                    new SqlParameter("@EndDate", SqlDbType.Date) { Value = benefitEndDate }
                    // new SqlParameter("@HealthcareProviderInfoXml", SqlDbType.Xml) { Value = healthcareProviderInfo?.AsXml() },
                    // new SqlParameter("@PolicyValue", SqlDbType.Money) { Value = (planBenefitAmount == null) ? DBNull.Value : planBenefitAmount }
                );
            }
            #endregion
        }
    }
}
