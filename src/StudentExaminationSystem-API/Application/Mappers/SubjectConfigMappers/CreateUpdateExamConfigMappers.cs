using Application.DTOs.SubjectExamConfigDtos;
using Domain.Models;
namespace Application.Mappers.SubjectConfigMappers;

public static class CreateSubjectExamConfigMappers
{
    public static SubjectExamConfig ToEntity(this CreateUpdateSubjectExamConfigAppDto dto, int id)
    {
        return new SubjectExamConfig
        {
            SubjectId = id,
            DurationMinutes = (int)dto.DurationMinutes!,
            TotalQuestions = (int)dto.TotalQuestions!,
            DifficultyProfileId = (int)dto.DifficultyProfileId!
        };
    }
    
    public static void MapUpdate(
        this CreateUpdateSubjectExamConfigAppDto dto,
        SubjectExamConfig subjectExamConfig)
    {
        subjectExamConfig.DurationMinutes = (int)dto.DurationMinutes!;
        subjectExamConfig.TotalQuestions = (int)dto.TotalQuestions!;
        subjectExamConfig.DifficultyProfileId = (int)dto.DifficultyProfileId!;
    }
}
