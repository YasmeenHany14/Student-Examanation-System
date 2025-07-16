using Application.Contracts;
using Application.DTOs.SubjectExamConfigDtos;
using Application.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Helpers.Extensions;
using WebApi.Helpers.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/subject/{id}/exam-config")]
public class SubjectExamConfigController(
    ISubjectExamConfigService configService,
    IValidator<CreateUpdateSubjectExamConfig> validator
) : ControllerBase
{
    // GET BY ID ASYNC
    [HttpGet(Name = "GetSubjectExamConfigById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await configService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    // POST ASYNC
    [HttpPost(Name = "CreateSubjectExamConfig")]
    [InputValidationFilter<CreateUpdateSubjectExamConfig>]
    public async Task<IActionResult> CreateAsync(int id, [FromBody] CreateUpdateSubjectExamConfig dto)
    {
        var result = await configService.CreateAsync(dto, id);
        if (!result.IsSuccess)
            return result.ToActionResult();
        return Created();
    }

    // PATCH ASYNC
    [HttpPatch(Name = "UpdateSubjectExamConfig")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] JsonPatchDocument<CreateUpdateSubjectExamConfig> patchDocument)
    {
        var config = await configService.GetByIdAsync(id);
        if (!config.IsSuccess)
            return config.ToActionResult();

        var configToPatch = config.Value;
        var (patchedDto, validationResult) = this.HandlePatch(configToPatch, patchDocument);
        if (!validationResult.IsSuccess)
            return validationResult.ToActionResult();

        var inputValid = await ValidationHelper.ValidateAndReportAsync(validator, patchedDto, "Input");
        if (!inputValid.IsSuccess)
            return inputValid.ToActionResult();

        var result = await configService.UpdateAsync(id, patchedDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return NoContent();
    }
}
