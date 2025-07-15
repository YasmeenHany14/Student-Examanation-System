using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Application.Helpers;
using Application.Mappers;
using Domain.DTOs;
using Domain.Repositories;
using Domain.UserContext;
using Shared.ResourceParameters;

namespace Application.Services;

public class ExamService(
    IUnitOfWork unitOfWork,
    IUserContext userContext
    ): IExamService
{
    public async Task<Result<PagedList<GetExamHistoryAppDto>>> GetAllAsync(
        ExamHistoryResourceParameters resourceParameters)
    {
        var userId = AccessResourceIdFilter.FilterResourceId<string?>(userContext);
        var exams = await unitOfWork.ExamHistoryRepository.GetAllExamHistoryAsync(
            resourceParameters, userId);
        return Result<PagedList<GetExamHistoryAppDto>>.Success(exams.ToListDto());
    }
    
    public async Task<Result<GetFullExamAppDto?>> GetFullExamAsync(int examId)
    {
        var exam = await unitOfWork.ExamHistoryRepository.GetAllQuestionHistoryAsync(examId);
        if (exam is null)
            return Result<GetFullExamAppDto?>.Failure(CommonErrors.NotFound);

        if(!AccessResourceIdFilter.IsAdminOrCanAccess(exam.userId, userContext))
            return Result<GetFullExamAppDto?>.Failure(AuthErrors.Forbidden);
        
        return Result<GetFullExamAppDto?>.Success(exam.MapToGetFullExamAppDto());
    }
}
