using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<PagedList<GetQuestionInfraDto>> GetAllAsync(QuestionResourceParameters resourceParameters);
    Task<IEnumerable<LoadExamQuestionInfraDto>> GetQuestionsForExamAsync(GenerateExamConfigDto generateExamConfig);
    Task<IEnumerable<CorrectAnswersInfraDto>> GetCorrectAnswersForQuestionsAsync(IEnumerable<int> questionIds);
    Task<Question?> GetEntityByIdAsync(int id);
}
