namespace Questionare.models
{
    public class Option
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
// Changed to public for broader accessibility
        public int QuestionId { get; set; }
        public bool? IsCorrect { get; set; }  // Now nullable
        public bool? IsDeleted { get; set; }

        //public virtual Question Question { get; set; }
    }
}
