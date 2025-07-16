using Application.Contracts;
using Application.DTOs.QuestionDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[ApiController]
[Route("api/question")]
public class QuestionController(
    IQuestionService questionService,
    IPaginationHelper<GetQuestionAppDto, QuestionResourceParameters> paginationHelper
    ) : ControllerBase
{
    // GetAllAsync Paged
    [HttpGet(Name = "GetAllQuestions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] QuestionResourceParameters resourceParameters)
    {
        var questions = await questionService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                questions.Value, resourceParameters, Response.Headers, Url, "GetAllQuestions");
        
        return questions.ToActionResult();
    }
    
    // CreateAsync
    [HttpPost(Name = "CreateQuestion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    [InputValidationFilter<CreateQuestionAppDto>]
    public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionAppDto questionDto)
    {
        var result = await questionService.CreateAsync(questionDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return Created();
    }
    
    // MakeQuestionNotActiveAsync
    [HttpGet("{questionId:int}/not-active", Name = "MakeQuestionNotActive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> MakeQuestionNotActiveAsync(int questionId)
    {
        var result = await questionService.MakeQuestionNotActiveAsync(questionId);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return Ok();
    }
    
    // DeleteAsync
    [HttpDelete("{questionId:int}", Name = "DeleteQuestion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> DeleteAsync(int questionId)
    {
        var result = await questionService.DeleteAsync(questionId);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return NoContent();
    }
}
