using Application.Common.ErrorAndResults;
using Application.DTOs.QuestionDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface IQuestionService
{
    Task<Result<PagedList<GetQuestionAppDto>>> GetAllAsync(QuestionResourceParameters resourceParameters);
    Task<Result<int>> CreateAsync(CreateQuestionAppDto questionAppDto);
    Task<Result<bool>> MakeQuestionNotActiveAsync(int questionId);
    Task<Result<bool>> DeleteAsync(int questionId);
}
