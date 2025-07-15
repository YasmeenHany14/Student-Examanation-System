using Application.Contracts;
using Application.DTOs.ExamDtos;
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
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] ExamHistoryResourceParameters resourceParameters)
    {
        var exams = await examService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                exams.Value, resourceParameters, Response.Headers, Url, "GetAllExams");
        
        return exams.ToActionResult();
    }
    
    [HttpGet("{examId}", Name = "GetFullExam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetFullExamAsync(int examId)
    {
        var exam = await examService.GetFullExamAsync(examId);
        return exam.ToActionResult();
    }
}
