namespace QuestApp.Models.Domain
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public string AnswerText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
