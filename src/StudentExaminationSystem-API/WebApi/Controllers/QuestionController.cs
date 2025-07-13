using Application.Contracts;
using Application.DTOs.QuestionDtos;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[ApiController]
[Route("api/question")]
public class QuestionController(
    IQuestionService questionService) : ControllerBase
{
    // GetAllAsync Paged
    [HttpGet(Name = "GetAllQuestions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] QuestionResourceParameters resourceParameters)
    {
        // Implementation for getting all questions with pagination
        return Ok(); // Placeholder
    }
    
    // CreateAsync
    [HttpPost(Name = "CreateQuestion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [InputValidationFilter<CreateQuestionAppDto>]
    public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionAppDto questionDto)
    {
        var result = await questionService.CreateAsync(questionDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return Created();
    }
    
    // DeleteAsync
}
