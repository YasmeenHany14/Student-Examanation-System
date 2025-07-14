using Application.Contracts;
using Application.DTOs.StudentDtos;
using Application.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController(
    IStudentService studentService,
    IUserService userService,
    IValidator<CreateStudentAppDto> createStudentValidator,
    IPaginationHelper<GetStudentByIdAppDto, StudentResourceParameters> paginationHelper)
    : ControllerBase
{
    // GET ALL ASYNC
    [HttpGet(Name = "GetAllStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] StudentResourceParameters resourceParameters)
    {
        var students = await studentService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                students.Value, resourceParameters, Response.Headers, Url, "GetAllStudents");

        return students.ToActionResult();
    }

    // GET BY ID ASYNC
    [HttpGet("{id}", Name = "GetStudentById")]
    // [TypeFilter(typeof(CanAccessResourceFilter), Arguments = [true])]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await studentService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    // POST ASYNC
    [HttpPost(Name = "AddStudent")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [InputValidationFilter<CreateStudentAppDto>]
    public async Task<IActionResult> AddAsync([FromBody] CreateStudentAppDto studentAppDto)
    {
        var result = await studentService.AddAsync(studentAppDto);
        if (!result.IsSuccess)
            return result.ToActionResult();
        return Created();
    }

    // Disable student => update is active status to true/false
    [HttpPatch("{id}", Name = "UpdateStudentStatus")]
    public async Task<IActionResult> ToggleStatusAsync(string id)
    {
        var result = await userService.ToggleStatusAsync(id);
        return result.ToActionResult();
    }
    
    // Add new student subject
    [HttpPost("{id}", Name = "AddStudentSubject")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddStudentSubjectAsync(
        string id, [FromBody] int subjectId)
    {
        var result = await studentService.AddStudentSubjectAsync(id, subjectId);
        if (!result.IsSuccess)
            return result.ToActionResult();
        return Created();
    }
}
