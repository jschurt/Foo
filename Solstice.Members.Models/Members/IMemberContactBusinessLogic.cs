using Solstice.Members.MemberContactModels;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberContactBusinessLogic
    {
        Task<MemberContactInfo> GetMemberContactAsync(int personId);
        [CodeGenerationAllowAnonymous]
        Task UpdateMemberEmailAsync(int personId, string emailAddress);
    }
}
