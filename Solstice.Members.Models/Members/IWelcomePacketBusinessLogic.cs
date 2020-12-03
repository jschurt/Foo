using Solstice.Members.MemberAddressModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IWelcomePacketBusinessLogic
    {
        [CodeGenerationAllowAnonymous]
        Task GenerateWelcomePacketsInQueueAsync();
        
        [CodeGenerationAllowAnonymous]
        Task QueueWelcomePacketsForAllEligibleMembersAsync();
    }
}
