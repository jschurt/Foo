using Solstice.Members.MemberIdCardModels;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberIdCardBusinessLogic
    {
        Task<GetMemberIdCardResult> GetMemberIdCardAsync(int personId, IdCardFormat format);
        Task<bool> IsMemberIdCardEligibleAsync(int personId);
    }
}
