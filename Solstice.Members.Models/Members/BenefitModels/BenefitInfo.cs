using System;

namespace Solstice.Members.BenefitModels
{
    public class BenefitInfo
    {
        public PlanInfo Plan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal MonthlyPremium { get; set; }

        public MemberInfo[] Members { get; set; }
    }
}
