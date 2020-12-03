namespace Solstice.Members.BenefitModels
{
    public class ScheduleOfBenefitsInfo
    {
        public enum LanguageID
        {
            Unknown = 0,
            English = 145,
            Spanish = 477
        }

        public int PlanId { get; set; }

        public string DisplayName { get; set; }
        public string DocumentUrl { get; set; }
        public LanguageID Language { get; set; }
    }
}
