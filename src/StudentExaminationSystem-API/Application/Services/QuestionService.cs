using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.QuestionDtos;
using Application.Helpers;
using Application.Mappers.QuestionMappers;
using Domain.Repositories;
using FluentValidation;
using Shared.ResourceParameters;

namespace Application.Services;

public class QuestionService(
    IUnitOfWork unitOfWork,
    IValidator<CreateQuestionAppDto> createQuestionValidator
    ) : IQuestionService
{
    public async Task<Result<PagedList<GetQuestionAppDto>>> GetAllAsync(
        QuestionResourceParameters resourceParameters)
    {
        var questions = await unitOfWork.QuestionRepository.GetAllAsync(resourceParameters);
        return Result<PagedList<GetQuestionAppDto>>.Success(questions.ToListDto());
    }

    public async Task<Result<int>> CreateAsync(CreateQuestionAppDto questionAppDto)
    {
        var validationResult =
            await ValidationHelper.ValidateAndReportAsync(
                createQuestionValidator, questionAppDto, "CreateBusiness");

        if (!validationResult.IsSuccess)
            return Result<int>.Failure(validationResult.Error);
        
        var question = questionAppDto.ToEntity();
        await unitOfWork.QuestionRepository.AddAsync(question);
        
        var result = unitOfWork.SaveChangesAsync();
        if (result.Result <= 0)
            return Result<int>.Failure(CommonErrors.InternalServerError());

        return Result<int>.Success(question.Id);
    }

    public async Task<Result<bool>> MakeQuestionNotActiveAsync(int questionId)
    {
        var question = await unitOfWork.QuestionRepository.GetEntityByIdAsync(questionId);
        if (question == null)
            return Result<bool>.Failure(CommonErrors.NotFound());

        question.IsActive = false;
        unitOfWork.QuestionRepository.UpdateAsync(question);
        
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int questionId)
    {
        var question = await unitOfWork.QuestionRepository.GetEntityByIdAsync(questionId);
        if (question == null)
            return Result<bool>.Failure(CommonErrors.NotFound());
        
        unitOfWork.QuestionRepository.DeleteAsync(question);
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        return Result<bool>.Success(true);
    }
}
