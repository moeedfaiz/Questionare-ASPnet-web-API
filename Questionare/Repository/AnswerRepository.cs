using Microsoft.EntityFrameworkCore;
using Questionare.models;
namespace Questionare.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            return await _context.Answers.ToListAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task<Answer> CreateAnswerAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task UpdateAnswerAsync(Option answer)
        {
            _context.Entry(answer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer != null)
            {
                answer.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> QuestionExists(int questionId)
        {
            return await _context.Questions.AnyAsync(q => q.QuestionId == questionId);
        }

        public async Task<bool> OptionExists(int questionId, int optionId)
        {
            return await _context.Options.AnyAsync(o => o.OptionId == optionId && o.QuestionId == questionId);
        }





        public Task UpdateAnswerAsync(Answer answer)
        {
            throw new NotImplementedException();
        }

    }
}
