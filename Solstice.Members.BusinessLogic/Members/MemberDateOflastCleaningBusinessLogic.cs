using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solstice.Members.MemberDateOfLastCleaningModels;
using Solstice.DAL.Workbench;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Solstice.Members
{
    sealed class MemberDateOflastCleaningBusinessLogic : IMemberDateOfLastCleaningBusinessLogic
    {
        private IWorkbenchDbContext Db { get; }

        public MemberDateOflastCleaningBusinessLogic(IWorkbenchDbContext workbenchDbContext)
        {
            Db = workbenchDbContext;
        }
        public async Task<MemberDateOfLastCleaning> GetMemberDateOfLastCleaningAsync(int personId)
        {
            var query =
                from d in Db.Members_vw_DateOfLastCleaning
                where d.ClaimPersonID == personId
                select new MemberDateOfLastCleaning
                {
                    DateOfLastCleaning = d.LastServiceDate,
                    PersonID = Convert.ToInt32(d.ClaimPersonID)
                };

            var result = await query.SingleOrDefaultAsync();

            return result;
        }
    }
}
