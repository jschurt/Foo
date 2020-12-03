using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.Members.BenefitModels
{
    public class ValueAddonInfo
    {
        public int PlanID { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string DocumentUrl { get; set; }
        public string ValueAddonDescription { get; set; }
        public int DocumentID { get; set; }
    }
}
