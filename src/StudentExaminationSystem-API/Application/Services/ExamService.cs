using System.Security.Cryptography;
using Application.Common.Constants.Errors;
using Application.Common.Constants.ValidationMessages;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Application.DTOs.SubjectExamConfigDtos;
using Application.Helpers;
using Application.Mappers;
using Application.Mappers.QuestionMappers;
using Domain.DTOs;
using Domain.DTOs.ExamDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Domain.UserContext;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Shared.ResourceParameters;

namespace Application.Services;

public class ExamService(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    IValidator<GenerateExamRequestDto> generateValidator,
    IGenerateExamService generateService,
    ICacheExamService cacheService
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
            return Result<GetFullExamAppDto?>.Failure(CommonErrors.NotFound());

        if(!AccessResourceIdFilter.IsAdminOrCanAccess(exam.userId, userContext))
            return Result<GetFullExamAppDto?>.Failure(AuthErrors.Forbidden);
        
        return Result<GetFullExamAppDto?>.Success(exam.MapToGetFullExamAppDto());
    }
    
    public async Task<Result<LoadExamAppDto>> GetExamAsync(int subjectId)
    {
        var userId = userContext.UserId;
        var hiddenUserId = await unitOfWork.StudentRepository.GetHiddenUserIdAsync(userId.ToString());
        var validationObject = new GenerateExamRequestDto(subjectId, hiddenUserId);
        var validationResult = await ValidationHelper.ValidateAndReportAsync(generateValidator, validationObject, "Business");
        
        if (!validationResult.IsSuccess)
            return Result<LoadExamAppDto>.Failure(validationResult.Error);

        var examEntry = cacheService.GetExamEntryAsync(userId.ToString());
        
        if (examEntry.Value is not null && examEntry.Value.SubjectId != subjectId)
            return Result<LoadExamAppDto>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamSubjectMismatch));

        if (examEntry.Value is not null && examEntry.Value.ExamEndTime > DateTime.UtcNow)
            return await generateService.GetCachedExamEntryAsync(examEntry.Value);
            
        return await generateService.GenerateExamAsync(subjectId, hiddenUserId);

    }

}
