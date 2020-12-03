using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.Members.EnrollmentModels
{
    public enum ProcessFileMode
    {
        None = 0,

        /// <summary>
        /// ES members.
        /// </summary>
        EssentialSmileIndividual = 1,

        /// <summary>
        /// SI Members.
        /// </summary>
        SolsticeInsuranceIndividual = 2
    }
}
