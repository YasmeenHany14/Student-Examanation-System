using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Helpers
{
    public static class PatchHelper
    {
        public static (TDto PatchedDto, Result ValidationResult) HandlePatch<TDto>(
            this ControllerBase controller,
            TDto originalDto,
            JsonPatchDocument<TDto> patchDoc)
            where TDto : BaseDto, new()
        {
            patchDoc.ApplyTo(originalDto, controller.ModelState);
            if (!controller.ModelState.IsValid)
            {
                return (originalDto, Result.Failure(CommonErrors.ValidationError));
            }
            return (originalDto, Result.Success());
        }
    }
}

