using Domain.DTOs.QuestionDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<PagedList<GetQuestionInfraDto>> GetAllAsync(QuestionResourceParameters resourceParameters);
}
