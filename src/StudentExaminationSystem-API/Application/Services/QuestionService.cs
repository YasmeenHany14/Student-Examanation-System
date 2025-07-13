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
    public Task<Result<PagedList<GetQuestionAppDto>>> GetAllAsync(QuestionResourceParameters resourceParameters)
    {
        throw new NotImplementedException();
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
            return Result<int>.Failure(CommonErrors.InternalServerError);

        return Result<int>.Success(question.Id);
    }
}
