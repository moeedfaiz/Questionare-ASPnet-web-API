using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionare.models
{
    [Table("tblCorrectAnswer")]
    public class Answer
    {
        [Key]

        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }

        public bool IsCorrect { get; set; }

        public bool? IsDeleted { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
