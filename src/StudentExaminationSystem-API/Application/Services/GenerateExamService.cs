using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionChoiceDtos;
using Application.Mappers;
using AutoMapper;
using Domain.DTOs.ExamDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Domain.UserContext;

namespace Application.Services;

public class GenerateExamService(
    IUnitOfWork unitOfWork,
    ICacheExamService cacheService,
    IUserContext userContext,
    INotificationsService notificationsService,
    IMapper mapper
    ) : IGenerateExamService
{
    public async Task<Result<LoadExamAppDto>> GenerateExamAsync(int subjectId, int userId)
    {
        var examConfig = await unitOfWork.SubjectExamConfigRepository.GetConfigToGenerateExamAsync(subjectId);
        // calculate number of questions based on exam config
        if (examConfig is null)
            return Result<LoadExamAppDto>.Failure(CommonErrors.InternalServerError());

        var generateConfig = new GenerateExamConfigDto(
            examConfig.TotalQuestions,
            new Dictionary<int, int>
            {
                { (int)Difficulty.Easy, examConfig.DifficultyProfile!.EasyPercentage },
                { (int)Difficulty.Medium, examConfig.DifficultyProfile.MediumPercentage },
                { (int)Difficulty.Hard, examConfig.DifficultyProfile.HardPercentage }
            });
        
        var questions = await unitOfWork.QuestionRepository.GetQuestionsForExamAsync(generateConfig);
        var loadExamQuestionInfraDtos = questions.ToList();
        
        if (!loadExamQuestionInfraDtos.Any() || loadExamQuestionInfraDtos.Count() < examConfig.TotalQuestions)
            return Result<LoadExamAppDto>.Failure(CommonErrors.InternalServerError());

        var examQuestions = mapper.Map<List<LoadExamQuestionAppDto>>(loadExamQuestionInfraDtos);
        var examEntryResult = await SaveExamEntryToDatabaseAsync(
            subjectId, userId, examConfig, examQuestions);
        if (!examEntryResult.IsSuccess)
            return Result<LoadExamAppDto>.Failure(examEntryResult.Error);
        var examEntryId = examEntryResult.Value;
        await notificationsService.NotifyExamStartedAsync(subjectId, userContext.UserId.ToString());
        
        return Result<LoadExamAppDto>.Success(
            new LoadExamAppDto
            {
                Id = examEntryId,
                SubjectId = subjectId,
                Questions = examQuestions,
                ExamEndTime = DateTime.UtcNow.AddMinutes(examConfig.DurationMinutes)
            });
    }
    
    private async Task<Result<int>> SaveExamEntryToDatabaseAsync(
        int subjectId,
        int userId,
        SubjectExamConfig examConfig,
        IEnumerable<LoadExamQuestionAppDto> questions)
    {
        var exam = new GeneratedExam()
        {
            SubjectId = subjectId,
            StudentId = userId,
            SubmittedAt = null,
            ExamTotalScore = questions.Count(),
            StudentScore = 0,
            QuestionHistory = questions.Select(q => new AnswerHistory
            {
                QuestionId = q.Id,
                IsCorrect = false,
                QuestionChoiceId = null,
                DisplayOrder = q.QuestionOrder
            }).ToList()
        };
        await unitOfWork.ExamHistoryRepository.AddAsync(exam);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError());

        var userIdString = userContext.UserId;
        cacheService.CacheExamEntry(exam.Id, subjectId, userIdString.ToString(), examConfig.DurationMinutes);
        return Result<int>.Success(exam.Id);
    }
    
    public async Task<Result<LoadExamAppDto>> GetCachedExamEntryAsync(
        ExamCacheEntryDto examCacheEntryDto)
    {
        var examEntry = await unitOfWork.ExamHistoryRepository.GetAllQuestionHistoryAsync(examCacheEntryDto.ExamId);
        if (examEntry is null)
            return Result<LoadExamAppDto>.Failure(CommonErrors.NotFound());

        return Result<LoadExamAppDto>.Success(examEntry.MapToLoadExamAppDto(examCacheEntryDto));
    }
}
