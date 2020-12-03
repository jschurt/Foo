using System.Collections.Generic;

namespace Solstice.Members.BenefitModels
{
    public class PlanInfo
    {
        public int PlanId { get; set; }
        public string Name { get; set; }
        public string PlanNumber { get; set; }
        public ProductInfo Product { get; set; }
        public PlanTypeInfo PlanType { get; set; }
        public IReadOnlyList<string> RiderPlanNumbers { get; set; }
    }
}
