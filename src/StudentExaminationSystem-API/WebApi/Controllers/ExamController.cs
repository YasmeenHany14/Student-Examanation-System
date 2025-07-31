using Application.Contracts;
using Application.DTOs.ExamDtos;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamController(
    IExamService examService,
    IPaginationHelper<GetExamHistoryAppDto, ExamHistoryResourceParameters> paginationHelper
    ) : ControllerBase
{
    // GET ALL ASYNC
    [HttpGet(Name = "GetAllExams")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [RequireAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] ExamHistoryResourceParameters resourceParameters)
    {
        var exams = await examService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                exams.Value, resourceParameters, Response.Headers, Url, "GetAllExams");

        return exams.ToActionResult();
    }
    
    [HttpGet("{examId}", Name = "GetFullExamHistory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [RequireAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetFullExamAsync(int examId)
    {
        var exam = await examService.GetFullExamAsync(examId);
        return exam.ToActionResult();
    }
    
    [HttpGet("subject/{subjectId}", Name = "GenerateExam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequireAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetExamAsync(int subjectId)
    {
        var exam = await examService.GetExamAsync(subjectId);
        return exam.ToActionResult();
    }
    
    [HttpPost(Name = "SubmitExam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> SubmitExamAsync([FromBody] LoadExamAppDto examDto)
    {
        var result = await examService.SubmitExamAsync(examDto);
        return result.ToActionResult();
    }
}
