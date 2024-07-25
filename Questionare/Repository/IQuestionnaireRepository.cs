using System.Collections.Generic;
using System.Threading.Tasks;
using Questionare.models;

public interface IQuestionnaireRepository
{
    Task<IEnumerable<Question>> GetAllQuestionsAsync();
    Task<Question> GetQuestionByIdAsync(int id);
    Task<Question> AddQuestionAsync(Question question);  // Correct type
    Task UpdateQuestionAsync(Question question);
    Task DeleteQuestionAsync(int id);
    Task AddOptionAsync(Option option);  // Confirm this is correctly implemented
    Task SaveChangesAsync();
}
