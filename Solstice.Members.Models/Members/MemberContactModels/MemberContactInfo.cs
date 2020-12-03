using Solstice.Core.Models;

namespace Solstice.Members.MemberContactModels
{
    public class MemberContactInfo
    {
        public int PersonId { get; set; }
        public string EmailAddress { get; set; }
        public PhoneNumber PrimaryPhoneNumber { get; set; }
    }
}
