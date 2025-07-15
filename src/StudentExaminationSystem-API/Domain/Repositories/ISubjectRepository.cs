using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface ISubjectRepository : IBaseRepository<Subject>
{
    Task<bool> CheckSubjectsExists(IEnumerable<int> subjectIds);
    Task<bool> CheckCodeUniqueAsync(string code, int? id = null);
    Task<PagedList<GetSubjectInfraDto>> GetAllAsync(SubjectResourceParameters resourceParameters);
    Task<IEnumerable<GetSubjectInfraDto>> GetAllAsync();
    Task<GetSubjectInfraDto?> GetByIdAsync(int id);
    Task<Subject?> GetEntityByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}
