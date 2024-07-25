namespace Questionare.DTO
{
    public class UpdateQuestionDTO
    {
        public int QuestionId { get; set; }  // Only needed if you're updating an existing question
        public string Text { get; set; }
        public string Category { get; set; }
        public List<UpdateOptionDTO> Options { get; set; }
    }
}
