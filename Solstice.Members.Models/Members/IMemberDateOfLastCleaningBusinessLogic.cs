using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solstice.Members.MemberDateOfLastCleaningModels;

namespace Solstice.Members
{
    [CodeGeneration]
    public interface IMemberDateOfLastCleaningBusinessLogic
    {
        Task<MemberDateOfLastCleaning> GetMemberDateOfLastCleaningAsync(int personId);
    }
}
