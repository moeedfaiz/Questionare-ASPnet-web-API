using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Questionare.DTO;
using Questionare.models;  // If necessary for mapping or direct access
using Questionare.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _repository;

        public AnswerController(IAnswerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAnswers()
        {
            try
            {
                var answers = await _repository.GetAllAnswersAsync();

                var answerDtos = answers.Select(a => new AnswerDTO
                {
                    OptionId = a.OptionId,
                    QuestionId = a.QuestionId,
                    IsCorrect = a.IsCorrect
                }).ToList();

                return Ok(answerDtos);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAnswer(int id)
        {
            var answer = await _repository.GetAnswerByIdAsync(id);
            
            if (answer == null)
            {
                return NotFound();
            }

            var answerDto = new AnswerDTO
            {
                OptionId = answer.OptionId,
                QuestionId = answer.QuestionId,
                IsCorrect = answer.IsCorrect
            };

            return Ok(answerDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostAnswer(AnswerDTO answerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the question exists
            var questionExists = await _repository.QuestionExists(answerDto.QuestionId);
            if (!questionExists)
            {
                return NotFound("Question does not exist in the system.");
            }

            // Check if the option exists for this question
            var optionExists = await _repository.OptionExists(answerDto.QuestionId, answerDto.OptionId);
            if (!optionExists)
            {
                return NotFound("The specified option does not exist for this question.");
            }

            try
            {
                var answer = new Answer
                {
                    OptionId = answerDto.OptionId,
                    QuestionId = answerDto.QuestionId,
                    IsCorrect = answerDto.IsCorrect
                };

                var createdAnswer = await _repository.CreateAnswerAsync(answer);
                if (createdAnswer == null)
                {
                    return NotFound("Unable to create the answer, please verify the data and try again.");
                }

                return CreatedAtAction("GetAnswer", new { id = createdAnswer.OptionId }, new AnswerDTO
                {
                    OptionId = createdAnswer.OptionId,
                    QuestionId = createdAnswer.QuestionId,
                    IsCorrect = createdAnswer.IsCorrect
                });
            }
            catch (Exception ex)
            {
                // Log the exception details here to help with debugging
                // Consider using a logging framework or service like Serilog, NLog, or Application Insights
                //Logger.Error(ex, "Failed to post an answer"); // Example assuming a Logger instance is available
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, AnswerDTO answerDto)
        {
            if (id != answerDto.OptionId)
            {
                return BadRequest();
            }

            var answer = await _repository.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            answer.QuestionId = answerDto.QuestionId;
            answer.IsCorrect = answerDto.IsCorrect;

            await _repository.UpdateAnswerAsync(answer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var question = await _repository.GetAnswerByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            await _repository.DeleteAnswerAsync(id);
            return NoContent();
        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteQuestion(int id)
        //{
        //    var question = await _repository.GetQuestionByIdAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    await _repository.DeleteQuestionAsync(id);
        //    return NoContent();
        //}
    }
}
