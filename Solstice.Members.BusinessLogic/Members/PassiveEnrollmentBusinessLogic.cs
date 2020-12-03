using Solstice.DAL;
using Solstice.DAL.Marketplace;
using Solstice.Members.EnrollmentModels;
using Solstice.Members.PassiveEnrollmentModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solstice.DAL.Marketplace.Models;
using static Solstice.Members.EnrollmentModels.LookupAttributes;

namespace Solstice.Members
{
    sealed class PassiveEnrollmentBusinessLogic : IPassiveEnrollmentBusinessLogic
    {
        private readonly ISqlUtility<IMarketplaceDbContext> _sqlUtility;
        private readonly IMarketplaceDbContext _marketplaceDbContext;
        private readonly IEnrollmentBusinessLogic _enrollmentBusinessLogic;

        public PassiveEnrollmentBusinessLogic(
            ISqlUtility<IMarketplaceDbContext> sqlUtility,
            IMarketplaceDbContext marketplaceDbContext,
            IEnrollmentBusinessLogic enrollmentBusinessLogic
            )
        {
            _sqlUtility = sqlUtility;
            _marketplaceDbContext = marketplaceDbContext;
            _enrollmentBusinessLogic = enrollmentBusinessLogic;
        }

        public async Task PassivelyEnrollAllEligibleMembersAsync()
        {
            // FR-G-05
            // Create an executable, that can be ran at any time, which will gather eligible subscribers and dependents             
            var membersToPassivelyEnroll = await GetMembersToPassivelyEnrollAsync();

            //Get distinct list of planIds from passive members to enroll
            var distinctPlanIds = membersToPassivelyEnroll
                .Select(i => (decimal)i.PassiveEnrollmentPlanID)
                .Distinct()
                .ToList();

            // use distinct plan Ids to get other needed benefit information
            var benefitInfoHelper = await (
                from pl in _marketplaceDbContext.Plan
                join bp in _marketplaceDbContext.BillingPeriod on pl.BillingPeriodID equals bp.BillingPeriodID
                where distinctPlanIds.Contains(pl.PlanID)
                select new
                {
                    PlanID = (int)pl.PlanID,
                    bp.StartDate,
                    bp.EndDate,
                    bp.BillingPeriodID
                })
                .ToDictionaryAsync(k => k.PlanID, v => new
                {
                    v.StartDate,
                    v.EndDate,
                    v.BillingPeriodID
                });

            // group members to passively enroll by family
            var families =
                from m in membersToPassivelyEnroll
                group m by m.GroupEmployeeID into g
                select new
                {
                    GroupEmployeeID = g.Key,
                    Members =
                        from m2 in g
                        orderby m2.IsSubscriber descending
                        select m2
                };

            // passively enroll each family
            foreach (var family in families)
            {
                using (var tran = TransactionScopeHelper.CreateForAsync())
                {
                    var subscriber = family.Members.First();
                    var benefitInfo = benefitInfoHelper[subscriber.PassiveEnrollmentPlanID];

                    // FR-G-08:
                    // Passive enrollment will create the following records: GroupProductBenefitPeriod, GroupPlan
                    // https://devops.solsticebenefits.com/tfs/DefaultTFSCollection/_git/Marketplace?path=%2FMarketplace.BLL%2FAreas%2FControls%2FEligibilityUploadIndividualControlBusinessLogic.cs&version=GBdevelop&line=1957&lineStyle=plain&lineEnd=2066&lineStartColumn=17&lineEndColumn=18
                    int benefitPeriodId = await _enrollmentBusinessLogic.GetOrCreateBillingPeriodAsync(subscriber.GroupID, subscriber.PassiveEnrollmentPlanID, benefitInfo.StartDate, EnrollmentModels.ProcessFileMode.EssentialSmileIndividual);

                    // Create Event//
                    // https://devops.solsticebenefits.com/tfs/DefaultTFSCollection/_git/Marketplace?path=%2FMarketplace.BLL%2FAreas%2FCore%2FMembershipBusinessLogic.cs&version=GBdevelop&line=486&lineStyle=plain&lineEnd=486&lineStartColumn=20&lineEndColumn=31
                    var eventId = await _enrollmentBusinessLogic.CreateEventAsync(benefitPeriodId, subscriber.GroupEmployeeID, LookupAttributes.EventType.EmployeeBenefitsChange, LookupAttributes.EventUpdateMethodType.GroupAdministratorUpdate);

                    // add coverage to each member
                    foreach (var member in family.Members)
                    {
                        // Add Coverage: GroupPlanSubscriber, GroupPlanPerson
                        // https://devops.solsticebenefits.com/tfs/DefaultTFSCollection/_git/Marketplace?path=%2FMarketplace.BLL%2FAreas%2FControls%2FEligibilityUploadIndividualControlBusinessLogic.cs&version=GBdevelop&line=2072&lineStyle=plain&lineEnd=2072&lineStartColumn=17&lineEndColumn=37
                        await _enrollmentBusinessLogic.AddCoverageAsync(eventId, member.PersonID, member.PassiveEnrollmentPlanID, benefitInfo.StartDate, benefitInfo.EndDate);
                    }

                    // create PEM record for GroupEmployeeID
                    _marketplaceDbContext.PassivelyEnrolledMember.Add(new PassivelyEnrolledMember()
                    {
                        BillingPeriodID = benefitInfo.BillingPeriodID,
                        GroupEmployeeID = subscriber.GroupEmployeeID,
                        PassivelyEnrolledMemberStatusID = (int)PassivelyEnrolledMemberStatusKey.Awaiting_Approval
                    });

                    await _marketplaceDbContext.SaveChangesAsync();

                    // Complete Event //
                    // https://devops.solsticebenefits.com/tfs/DefaultTFSCollection/_git/Marketplace?path=%2FMarketplace.BLL%2FAreas%2FCore%2FMembershipBusinessLogic.cs&version=GBdevelop&line=562&lineStyle=plain&lineEnd=562&lineStartColumn=21&lineEndColumn=34
                    await _enrollmentBusinessLogic.CompleteEventAsync(eventId);

                    tran.Complete();
                }
            }
        }

        public async Task<List<PassiveEnrollmentMember>> GetMembersToPassivelyEnrollAsync()
        {
            return await _sqlUtility.ExecuteStoredProcedureAsync(
                "[Members].[usp_PassiveEnrollment_GetMembers]",
                null,
                async (cm) =>
                {
                    var members = new List<PassiveEnrollmentMember>();

                    using (var dr = await cm.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            members.Add(new PassiveEnrollmentMember()
                            {
                                GroupID = (decimal)dr["GroupID"],
                                MemberNumber = (string)dr["MemberNumber"],
                                CurrentPlanNumber = (string)dr["CurrentPlanNumber"],
                                CurrentPlanName = (string)dr["CurrentPlanName"],
                                PassiveEnrollmentPlanNumber = (string)dr["PassiveEnrollmentPlanNumber"],
                                PassiveEnrollmentPlanName = (string)dr["PassiveEnrollmentPlanName"],
                                AccountID = (int)dr["AccountID"],
                                IsAccountOnHold = (bool)dr["IsAccountOnHold"],
                                CurrentYearFirstInvoiceMonth = (DateTime)dr["CurrentYearFirstInvoiceMonth"],
                                CurrentYearLastInvoiceMonth = (DateTime)dr["CurrentYearLastInvoiceMonth"],
                                // CurrentYearPaidThruMonth = (DateTime?) dr["CurrentYearPaidThruMonth"],
                                GroupEmployeeID = (int)dr["GroupEmployeeID"],
                                PersonID = (int)dr["PersonID"],
                                IsSubscriber = (bool)dr["IsSubscriber"],
                                PassiveEnrollmentPlanID = (int)dr["PassiveEnrollmentPlanID"],
                            });
                        }
                    }

                    return members;
                });
        }
    }
}
