using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questionare.models;

public class QuestionnaireRepository : IQuestionnaireRepository
{
    private readonly AppDbContext _context;

    public QuestionnaireRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions
                             .Include(q => q.Options)
                             .Where(q => !q.IsDeleted)
                             .ToListAsync();
    }

    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        return await _context.Questions
                             .Include(q => q.Options)
                             .FirstOrDefaultAsync(q => q.QuestionId == id && !q.IsDeleted);
    }

    public async Task<Question> AddQuestionAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);

        if (question != null)
        {
            question.IsDeleted = true;
            foreach (var option in question.Options)
            {
                option.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("Question not found with the specified ID.");
        }
    }

    public async Task AddOptionAsync(Option option)
    {
        _context.Options.Add(option);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
