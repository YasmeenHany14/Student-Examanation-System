using Application.Contracts;
using Application.DTOs.DifficultyProfileDtos;
using Application.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.ResourceParameters;
using WebApi.Helpers;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;

namespace WebApi.Controllers;

[ApiController]
[Route("api/difficulty-profile")]
public class DifficultyProfileController(
    IDifficultyProfileService difficultyProfileService,
    IValidator<CreateUpdateDifficultyProfileAppDto> validator,
    IPaginationHelper<GetDifficultyProfileAppDto, DifficultyProfileResourceParameters> paginationHelper
    ) : ControllerBase
{
    // GET ALL ASYNC
    [HttpGet(Name = "GetAllDifficultyProfiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] DifficultyProfileResourceParameters resourceParameters)
    {
        var profiles = await difficultyProfileService.GetAllAsync(resourceParameters);
        paginationHelper
            .CreateMetaDataHeader(
                profiles.Value, resourceParameters, Response.Headers, Url, "GetAllDifficultyProfiles");
        
        return profiles.ToActionResult();
    }
    
    // GET ALL ASYNC WITHOUT PAGINATION
    [HttpGet("all", Name = "GetAllDifficultyProfilesDropdown")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> GetAllAsync()
    {
        var profiles = await difficultyProfileService.GetAllAsync();
        return profiles.ToActionResult();
    }
    
    // POST ASYNC
    [HttpPost(Name = "CreateDifficultyProfile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    [InputValidationFilter<CreateUpdateDifficultyProfileAppDto>]
    public async Task<IActionResult> CreateAsync(int id, [FromBody] CreateUpdateDifficultyProfileAppDto dto)
    {
        var result = await difficultyProfileService.CreateAsync(dto, id);
        if (!result.IsSuccess)
            return result.ToActionResult();
        return Created();
    }

    // PATCH ASYNC
    [HttpPatch("/{id}" ,Name = "UpdateDifficultyProfile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] JsonPatchDocument<CreateUpdateDifficultyProfileAppDto> patchDocument)
    {
        var profile = await difficultyProfileService.GetByIdAsync(id);
        if (!profile.IsSuccess)
            return profile.ToActionResult();

        var profileToPatch = profile.Value;
        var (patchedDto, validationResult) = this.HandlePatch(profileToPatch, patchDocument);
        if (!validationResult.IsSuccess)
            return validationResult.ToActionResult();

        var inputValid = await ValidationHelper.ValidateAndReportAsync(validator, patchedDto, "Input");
        if (!inputValid.IsSuccess)
            return inputValid.ToActionResult();

        var result = await difficultyProfileService.UpdateAsync(id, patchedDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return NoContent();
    }
    
    // DELETE ASYNC
    [HttpDelete("/{id}", Name = "DeleteDifficultyProfile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = IdentityData.AdminUserPolicyName)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await difficultyProfileService.DeleteAsync(id);
        if (!result.IsSuccess)
            return result.ToActionResult();
        return NoContent();
    }
}
