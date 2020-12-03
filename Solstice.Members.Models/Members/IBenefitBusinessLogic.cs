using Solstice.Members.BenefitModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IBenefitBusinessLogic
    {
        Task<List<BenefitInfo>> GetAllBenefitsAsync(int personId, bool includeFamily, bool excludePremium = false);
        Task<List<BenefitInfo>> GetCurrentBenefitsAsync(int personId, bool includeFamily, bool excludePremium = false);
        Task<List<ProductInfo>> GetAllProductsAsync(int personId, bool includeFamily);
        Task<List<ProductInfo>> GetCurrentProductsAsync(int personId, bool includeFamily);
        Task<List<PlanInfo>> GetAllPlansAsync(int personId, bool includeFamily);
        Task<List<PlanInfo>> GetCurrentPlansAsync(int personId, bool includeFamily);
        Task<List<ScheduleOfBenefitsInfo>> GetCurrentScheduleOfBenefitsAsync(int personId);
        Task<bool> HasCurrentDavisVisionPlan(int personId);
        Task<List<ValueAddonInfo>> GetCurrentValueAddonDocumentsAsync(int personId);
    }
}
