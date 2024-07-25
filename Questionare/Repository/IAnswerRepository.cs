using Questionare.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questionare.Repositories
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAllAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(int id);
        Task<Answer> CreateAnswerAsync(Answer answer);
        Task UpdateAnswerAsync(Answer answer);
        Task DeleteAnswerAsync(int id);
        Task<bool> QuestionExists(int questionId);
        Task<bool> OptionExists(int questionId, int optionId);
    }
}
