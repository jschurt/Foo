using Solstice.Members.MemberAddressModels;
using System.Threading.Tasks;
using Solstice.Core.Models;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberAddressBusinessLogic
    {
        Task<MemberAddressInfo> GetMemberAddressAsync(int personId);
        Task<GeographicCoordinates> GetCoordinatesFromPrimaryAddressAsync(int personId);
    }
}
