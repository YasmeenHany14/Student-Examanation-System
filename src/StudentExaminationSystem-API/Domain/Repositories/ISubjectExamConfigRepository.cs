using Domain.DTOs.SubjectExamConfigDtos;
using Domain.Models;

namespace Domain.Repositories;

public interface ISubjectExamConfigRepository : IBaseRepository<SubjectExamConfig>
{
    Task<GetSubjectExamConfigInfraDto?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<SubjectExamConfig?> FindAsync(int id);
}
