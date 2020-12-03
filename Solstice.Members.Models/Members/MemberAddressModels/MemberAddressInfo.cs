using Solstice.Core.Models;

namespace Solstice.Members.MemberAddressModels
{
    public class MemberAddressInfo
    {
        public int PersonId { get; set; }
        public StreetAddress PrimaryAddress { get; set; }
    }
}
