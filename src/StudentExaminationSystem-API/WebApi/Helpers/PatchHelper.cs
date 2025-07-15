using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.DTOs;
using Application.DTOs.SubjectsDtos;
using Application.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers;

public static class PatchHelper
{
    public static (TUpdateDto PatchedDto, Result ValidationResult) HandlePatch<TGetDto, TUpdateDto>(
        this ControllerBase controller,
        TGetDto originalDto,
        JsonPatchDocument<TUpdateDto> patchDoc)
        where TGetDto : AppBaseDto
        where TUpdateDto : AppBaseDto, new ()
    {
        var updateDto = originalDto.MapTo<TGetDto, TUpdateDto>();
        patchDoc.ApplyTo(updateDto, controller.ModelState);
        if (!controller.ModelState.IsValid)
        {
            return (updateDto, Result.Failure(CommonErrors.ValidationError()));
        }
        return (updateDto, Result.Success());
    }
}
