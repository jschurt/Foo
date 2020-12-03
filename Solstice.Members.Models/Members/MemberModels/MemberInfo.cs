using Solstice.Core.Models;
using System;

namespace Solstice.Members.MemberModels
{
    public class MemberInfo
    {
        public int PersonId { get; set; }
        public string MemberNumber { get; set; }
        public string DisplayMemberNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderID Gender { get; set; }
        public bool IsSubscriber { get; set; }
        public string LastFourSsn { get; set; }
        public string EmailAddress { get; set; }
        public string Relationship { get; set; }
    }
}
