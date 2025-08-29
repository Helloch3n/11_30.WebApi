using _11_30.Domain.Entities.QuestionBank;

namespace _11_30.Domain.Repositories
{
    public interface IQuestionBankRepository
    {
        Task<QuestionBank> GetByIdAsync(Guid id);
        Task<List<QuestionBank>> GetAllAsync();
        Task<Question> GetQuestionByIdAsync(Guid id);
        Task<Question> GetQuestionByTitleAsync(string title);
    }
}
