using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<PagedList<GetQuestionInfraDto>> GetAllAsync(QuestionResourceParameters resourceParameters);
    Task<IEnumerable<LoadExamQuestionInfraDto>> GetQuestionsForExamAsync(GenerateExamConfigDto generateExamConfig);
    Task<IEnumerable<LoadExamQuestionInfraDto>> GetRunningExamQuestionsAsync(int examId);
}
