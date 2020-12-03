using Solstice.Members.EnrollmentModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.Members
{
    public interface IEnrollmentBusinessLogic
    {
        Task AddCoverageAsync(int eventId, int personId, int planId, DateTime benefitStartDate, DateTime benefitEndDate);
        Task CompleteEventAsync(int eventId, bool process = false);
        Task<int> CreateEventAsync(int benefitPeriodId, int groupEmployeeId, LookupAttributes.EventType type, LookupAttributes.EventUpdateMethodType updateMethod, LookupAttributes.QualifyingLifeEventReason? reason = null, DateTime? eventDate = null, DateTime? userDueDate = null, DateTime? changeEffectiveDate = null, bool setAsCompleted = false);
        Task<int> GetOrCreateBillingPeriodAsync(decimal groupID, int planID, DateTime benefitStartDate, ProcessFileMode mode);
    }
}
