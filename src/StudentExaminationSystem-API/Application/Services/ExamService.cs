using Application.Common.Constants.Errors;
using Application.Common.Constants.ValidationMessages;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs;
using Application.DTOs.ExamDtos;
using Application.Helpers;
using Application.Mappers;
using AutoMapper;
using Domain.DTOs.ExamDtos;
using Domain.Interfaces;
using Domain.Models;
using Domain.Repositories;
using Domain.UserContext;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class ExamService(
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    IValidator<GenerateExamRequestDto> generateValidator,
    IGenerateExamService generateService,
    ICacheExamService cacheService,
    IPublisher publisher,
    IMapper mapper
    ): IExamService
{
    public async Task<Result<PagedList<GetExamHistoryAppDto>>> GetAllAsync(
        ExamHistoryResourceParameters resourceParameters)
    {
        var userId = AccessResourceIdFilter.FilterResourceId<string?>(userContext);
        var exams = await unitOfWork.ExamHistoryRepository.GetAllExamHistoryAsync(
            resourceParameters, userId);
        
        var mappedExams = mapper.Map<List<GetExamHistoryAppDto>>(exams.Data);
        return Result<PagedList<GetExamHistoryAppDto>>.Success(new PagedList<GetExamHistoryAppDto>(exams.Pagination, mappedExams));
    }
    
    public async Task<Result<GetFullExamAppDto?>> GetFullExamAsync(int examId)
    {
        var exam = await unitOfWork.ExamHistoryRepository.GetAllQuestionHistoryAsync(examId);
        if (exam is null)
            return Result<GetFullExamAppDto?>.Failure(CommonErrors.NotFound());
        
        if (exam.ExamStatus == Domain.Enums.ExamStatus.Running)
            return Result<GetFullExamAppDto?>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamNotSubmitted));
        
        if (exam.ExamStatus == Domain.Enums.ExamStatus.PendingEvaluation)
            return Result<GetFullExamAppDto?>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamPendingEvaluation));

        if(!AccessResourceIdFilter.IsAdminOrCanAccess(exam.userId, userContext))
            return Result<GetFullExamAppDto?>.Failure(AuthErrors.Forbidden);
        
        var mappedExam = mapper.Map<GetFullExamAppDto>(exam);
        return Result<GetFullExamAppDto?>.Success(mappedExam);
    }
    
    public async Task<Result<LoadExamAppDto>> GetExamAsync(int subjectId)
    {
        var userId = userContext.UserId;
        var hiddenUserId = await unitOfWork.StudentRepository.GetHiddenUserIdAsync(userId.ToString());


        var examEntry = cacheService.GetExamEntryAsync(userId.ToString());
        
        if (examEntry.Value is not null && examEntry.Value.SubjectId != subjectId)
            return Result<LoadExamAppDto>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamSubjectMismatch));

        if (examEntry.Value is not null && examEntry.Value.ExamEndTime > DateTime.UtcNow)
            return await generateService.GetCachedExamEntryAsync(examEntry.Value);
            
        var validationObject = new GenerateExamRequestDto(subjectId, hiddenUserId);
        var validationResult = await ValidationHelper.ValidateAndReportAsync(generateValidator, validationObject, "Business");
        
        if (!validationResult.IsSuccess)
            return Result<LoadExamAppDto>.Failure(validationResult.Error);
        return await generateService.GenerateExamAsync(subjectId, hiddenUserId);
    }

    public async Task<Result<bool>> SubmitExamAsync(LoadExamAppDto ExamDto, bool entryExpired = false)
    {
        var userId = userContext.UserId;
        if (!entryExpired)
        {
            var examEntry = cacheService.GetExamEntryAsync(userId.ToString());
            if (examEntry.Value is null)
                return Result<bool>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamNotFound));
            if (examEntry.Value.SubjectId != ExamDto.SubjectId)
                return Result<bool>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamSubjectMismatch));
        }
        
        await SendExamToEvaluationService(ExamDto);
        
        var examEntity = await unitOfWork.ExamHistoryRepository.GetExamForUpdate(ExamDto.Id);
        examEntity.SubmittedAt = DateTime.UtcNow;
        examEntity.ExamStatus = Domain.Enums.ExamStatus.PendingEvaluation;
        examEntity.MapUpdate(ExamDto);
        examEntity = (await MarkExamCorrectAnswers(examEntity)).Value;
        
        unitOfWork.ExamHistoryRepository.UpdateAsync(examEntity);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        
        cacheService.RemoveExamEntry(userId.ToString());
        return Result<bool>.Success(true);
    }
    
    private async Task<Result<bool>> SendExamToEvaluationService(LoadExamAppDto examDto)
    {
        var questionIds = examDto.Questions.Select(q => q.Id).ToList();
        var answers  = examDto.Questions.Select(q => new EvaluateQuestionDto()
        {
            QuestionId = q.Id,
            AnswerId = q.Choices.FirstOrDefault(c => c.IsSelected)?.Id ?? 0,
        });
        var examAnswers = await unitOfWork.QuestionRepository.GetCorrectAnswersForQuestionsAsync(questionIds);
        await publisher.PublishExamAsync(examAnswers, examDto.Id, answers);
        return Result<bool>.Success(true);
    }

    private async Task<Result<GeneratedExam>> MarkExamCorrectAnswers(GeneratedExam exam)
    {
        var questionIds = exam.QuestionHistory!.Select(qh => qh.QuestionId).ToList();
        var examAnswers = await unitOfWork.QuestionRepository.GetCorrectAnswersForQuestionsAsync(questionIds);
        
        foreach (var question in exam.QuestionHistory!)
        {
            var correctAnswer = examAnswers.FirstOrDefault(c => c.QuestionId == question.QuestionId)?.CorrectAnswerId;
            if (correctAnswer.HasValue)
                question.IsCorrect = question.QuestionChoiceId == correctAnswer.Value;
        }
        return Result<GeneratedExam>.Success(exam);
    }
    
    public async Task<Result<(int, int)>> SaveExamEvaluationAsync(ExamEvaluationDto examEvaluationDto)
    {
        var exam = await unitOfWork.ExamHistoryRepository.GetExamForUpdate(examEvaluationDto.ExamId);
        if (exam is null)
            return Result<(int, int)>.Failure(CommonErrors.NotFound());

        if (exam.ExamStatus != Domain.Enums.ExamStatus.PendingEvaluation)
            return Result<(int, int)>.Failure(CommonErrors.BadRequest(ExamValidationErrorMessages.ExamNotPendingEvaluation));

        exam.StudentScore = examEvaluationDto.TotalScore;
        exam.ExamStatus = Domain.Enums.ExamStatus.Completed;
        
        unitOfWork.ExamHistoryRepository.UpdateAsync(exam);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<(int, int)>.Failure(CommonErrors.InternalServerError());
        
        return Result<(int, int)>.Success((exam.SubjectId, exam.StudentId));
    }
    
    public async Task OnExamEntryExpired(ExamCacheEntryDto entryDto)
    {
        var examEntry = await generateService.GetCachedExamEntryAsync(entryDto);
        await SubmitExamAsync(examEntry.Value, true);
    }
}
