using Solstice.Members.MemberModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberBusinessLogic
    {
        Task<List<MemberInfo>> GetDependentsAsync(int parentPersonId);
        Task<MemberInfo> GetMemberAsync(int personId);
        Task<int> GetMemberPersonIdAsync(int userId);
        [CodeGenerationAllowAnonymous]
        Task<MemberGroupInfo> GetMemberGroupInfoAsync(int personId);
        [CodeGenerationAllowAnonymous]
        Task<MemberInfo> GetMemberForUserRegistrationAsync(DateTime dateOfBirth, string zipCode, string memberNumber, string socialSecurityNumber, string partnerTemplateName);
        Task<string> GetPartnerTemplateAsync(int personId);
        Task<List<MemberInfo>> GetFamilyAsync(int memberPersonId);
    }
}
