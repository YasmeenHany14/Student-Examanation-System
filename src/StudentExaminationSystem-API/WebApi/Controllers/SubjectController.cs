using Application.Contracts;
using Application.DTOs.SubjectsDtos;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[Route("api/subject")]
[ApiController]
public class SubjectController(
    ISubjectService subjectService,
    IPaginationHelper<GetSubjectAppDto, SubjectResourceParameters> paginationHelper
    ) : ControllerBase
{
    // CRUD
    // GET ALL ASYNC
    [HttpGet(Name = "GetAllSubjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] SubjectResourceParameters resourceParameters)
    {
        var subjects = await subjectService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                subjects.Value, resourceParameters, Response.Headers, Url, "GetAllSubjects");
        
        return subjects.ToActionResult();
    }
    
    // GET ALL ASYNC WITHOUT PAGINATION
    [HttpGet("all", Name = "GetAllSubjectsDropdown")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var subjects = await subjectService.GetAllAsync();
        return Ok(subjects);
    }
    
    // GET BY ID ASYNC
    [HttpGet("{id}", Name = "GetSubjectById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await subjectService.GetByIdAsync(id);
        return result.ToActionResult();
    }
    
    // POST ASYNC
    [HttpPost(Name = "CreateSubject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        CreateSubjectAppDto createDto)
    {
        var result = await subjectService.CreateAsync(createDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return Created();
    }
}
