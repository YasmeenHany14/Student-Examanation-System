using Application.Contracts;
using Application.DTOs.SubjectsDtos;
using Application.Helpers;
using Application.Validators.StudentValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[Route("api/subject")]
[ApiController]
public class SubjectController(
    ISubjectService subjectService,
    IPaginationHelper<GetSubjectAppDto, SubjectResourceParameters>  paginationHelper,
    IValidator<UpdateSubjectAppDto> updateValidator
    ) : ControllerBase
{
    // CRUD
    // GET ALL ASYNC
    [HttpGet(Name = "GetAllSubjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    // [TypeFilter(typeof(CanAccessResourceFilter), Arguments = [false])]
    public async Task<IActionResult> GetAllAsync()
    {
        var subjects = await subjectService.GetAllAsync();
        return subjects.ToActionResult();
    }
    
    // GET BY ID ASYNC
    [HttpGet("{id}", Name = "GetSubjectById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await subjectService.GetByIdAsync(id);
        return result.ToActionResult();
    }
    
    // POST ASYNC
    [HttpPost(Name = "CreateSubject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> CreateAsync(
        CreateSubjectAppDto createDto)
    {
        var result = await subjectService.CreateAsync(createDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return Created();
    }
    
    // PATCH ASYNC
    //TODO: TRY MAKING THIS CONTROLLER SMALLER
    [HttpPatch("{id}", Name = "UpdateSubject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> UpdateAsync(
        int id,
        JsonPatchDocument<UpdateSubjectAppDto> patchDocument)
    {
        var subjectResult = await subjectService.GetByIdAsync(id);
        if (!subjectResult.IsSuccess)
            return subjectResult.ToActionResult();

        var subjectToPatch = subjectResult.Value; // Assuming ToUpdateDto is not needed for OwnerDto
        var (patchedDto, validationResult) = this.HandlePatch(subjectToPatch, patchDocument);
        if (!validationResult.IsSuccess)
            return validationResult.ToActionResult();

        var inputValid = await ValidationHelper.ValidateAndReportAsync(updateValidator, patchedDto, "Input");
        if (!inputValid.IsSuccess)
            return inputValid.ToActionResult();

        var result = await subjectService.UpdateAsync(id, patchedDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return NoContent();
    }
    
    // DELETE ASYNC
    [HttpDelete("{id}", Name = "DeleteSubject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await subjectService.DeleteAsync(id);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return NoContent();
    }
}
