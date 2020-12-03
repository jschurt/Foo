using Solstice.Members.PassiveEnrollmentModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IPassiveEnrollmentBusinessLogic
    {
        Task PassivelyEnrollAllEligibleMembersAsync();
        Task<List<PassiveEnrollmentMember>> GetMembersToPassivelyEnrollAsync();
    }
}
