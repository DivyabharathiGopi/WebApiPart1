using System.ComponentModel.DataAnnotations;

namespace QuestApp.Models.Domain
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Title {  get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Tags { get; set; } = "Common";
    }
}
