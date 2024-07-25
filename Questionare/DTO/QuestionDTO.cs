using System.ComponentModel.DataAnnotations;

namespace Questionare.DTO
{
    public class QuestionDTO
    {
       
        //public int QuestionId { get; set; }  // Only needed if you're updating an existing question
        public string? Text { get; set; }
        public string Category { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Each question must have at least three options.")]
        public List<OptionDTO> Options { get; set; }
    }

}
