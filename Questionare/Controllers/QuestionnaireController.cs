using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Questionare.DTO;
using Questionare.models;
namespace Questionare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionnaireRepository _repository;

        public QuestionsController(IQuestionnaireRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult> GetQuestions()
        {
            var questions = await _repository.GetAllQuestionsAsync();
            return Ok(questions);
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _repository.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion([FromBody] QuestionDTO questionDto)
        {
            // Validate the incoming DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (questionDto.Options == null || questionDto.Options.Count < 3)
            {
                return BadRequest("Each question must have at least three options.");
            }

            try
            {
                // Map DTO to Entity
                var question = new Question
                {
                    Text = questionDto.Text,
                    Category = questionDto.Category,
                    IsDeleted = false,  // Assuming new questions are not deleted
                    Options = questionDto.Options.Select(o => new Option
                    {
                        OptionText = o.OptionText,
                        IsCorrect = false,  // Default to false, adjust based on your application needs
                        IsDeleted = o.IsDeleted
                    }).ToList()
                };

                // Add the question to the database via repository
                await _repository.AddQuestionAsync(question);

                // Return the newly created question with route to fetch it
                return CreatedAtAction(nameof(GetQuestion), new { id = question.QuestionId }, question);
            }
            catch (System.Exception ex)
            {
                // Log the exception here: consider using a logging library or framework
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }



        // PUT: api/Questions/5
        [HttpPut("{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] UpdateQuestionDTO questionDto)
        {
            if (questionId != questionDto.QuestionId)
            {
                return BadRequest("Mismatched question IDs");
            }

            var question = await _repository.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }

            question.Text = questionDto.Text;
            question.Category = questionDto.Category;

            // This list tracks which option IDs have been processed
            var processedOptionIds = new HashSet<int>();

            foreach (var optionDto in questionDto.Options)
            {
                var option = question.Options.FirstOrDefault(o => o.OptionId == optionDto.OptionId);
                if (option != null)
                {
                    // Update existing option
                    option.OptionText = optionDto.OptionText;
                    option.IsCorrect = optionDto.IsCorrect;
                    processedOptionIds.Add(option.OptionId); // Mark this ID as processed
                }
                else
                {
                    // Add new option if it does not exist
                    question.Options.Add(new Option
                    {
                        OptionText = optionDto.OptionText,
                        IsCorrect = optionDto.IsCorrect
                    });
                }
            }

            // Now, we do not remove any options. We only add or update.

            await _repository.UpdateQuestionAsync(question);
            await _repository.SaveChangesAsync();  // Ensure this method is properly implemented

            return Ok();
        }






        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _repository.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            await _repository.DeleteQuestionAsync(id);
            return NoContent();
        }
    }
}
