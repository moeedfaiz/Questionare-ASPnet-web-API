using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionare.models
{
    [Table("tblQuestions")]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string? Text { get; set; }
        public string? Category { get; set; }

        public bool IsDeleted { get; set; }
        public List<Option> Options { get; set; }
    }
}
