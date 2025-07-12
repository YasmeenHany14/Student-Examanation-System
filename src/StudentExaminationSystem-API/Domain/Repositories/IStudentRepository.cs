using Domain.DTOs.StudentDtos;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<PagedList<GetStudentByIdInfraDto>> GetAllAsync(StudentResourceParameters resourceParameters);
    Task<GetStudentByIdInfraDto?> GetByIdAsync(string id);
}
