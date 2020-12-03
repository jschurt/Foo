using Solstice.Members.MemberVoeModels;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberVoeBusinessLogic
    {
        Task<VoeRequestResult> RequestVoeByFaxAsync(string memberNumber, string faxNumber, int productId, bool inNetwork);
    }
}
