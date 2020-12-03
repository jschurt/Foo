using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.Members.PassiveEnrollmentModels
{
    public class PassiveEnrollmentMember
    {
        public decimal GroupID { get; set; }
        public string MemberNumber { get; set; }
        public string CurrentPlanNumber { get; set; }
        public string CurrentPlanName { get; set; }
        public string PassiveEnrollmentPlanNumber { get; set; }
        public string PassiveEnrollmentPlanName { get; set; }
        public int AccountID { get; set; }
        public bool IsAccountOnHold { get; set; }
        public DateTime CurrentYearFirstInvoiceMonth { get; set; }
        public DateTime CurrentYearLastInvoiceMonth { get; set; }
        public DateTime? CurrentYearPaidThruMonth { get; set; }
        public int GroupEmployeeID { get; set; }
        public int PersonID { get; set; }
        public bool IsSubscriber { get; set; }
        public int PassiveEnrollmentPlanID { get; set; }
    }
}
