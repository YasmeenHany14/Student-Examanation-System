using Application.Contracts;
using Application.DTOs.StudentDtos;
using Application.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController(
    IStudentService studentService,
    IValidator<CreateStudentAppDto> createStudentValidator
    )
    : ControllerBase
{
    
    // GET BY ID ASYNC
    [HttpGet("{id}", Name = "GetStudentById")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok();
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
        return CreatedAtRoute("GetStudentById", new { id = result.Value.Id }, result.Value);
    }
}
