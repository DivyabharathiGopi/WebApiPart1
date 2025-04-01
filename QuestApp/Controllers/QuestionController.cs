using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestApp.Data;
using QuestApp.Models.Domain;

namespace QuestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestAppDbContext dbContext;

        //Constructor db class injection
        public QuestionController(QuestAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var questionsDomain = dbContext.questions.ToList();
            if(!questionsDomain.Any())
            {
                return NotFound("No Question found");
            }
            return Ok(questionsDomain);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute]Guid id)
        {
            var question = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);
            if(question == null)
            {
                return NotFound("Question Not Found");
            }
            return Ok(question);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] Question question)
        {
            if (question == null)
            {
                return BadRequest("Invalid Request Data");
            }

            question.CreatedDate = DateTime.UtcNow;
            dbContext.questions.Add(question);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = question.QuestionId }, question);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update([FromRoute] Guid id, [FromBody] Question question)
        {
            if (question == null || id!=question.QuestionId)
            {
                return BadRequest("Invalid or missing data");
            }

            var ExistingQuestion = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);

            if (ExistingQuestion == null)
            {
                return NotFound("Question not found");
            }

            ExistingQuestion.Title = question.Title;
            ExistingQuestion.Description = question.Description;
            ExistingQuestion.Tags = question.Tags;

            
            dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var ExistingQuestion = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);
            if (ExistingQuestion == null)
            {
                return NotFound("Question not found");
            }
            dbContext.questions.Remove(ExistingQuestion);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}
