using Domain.DTOs.StudentDtos;
using Domain.Models;

namespace Domain.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<GetStudentByIdInfraDto?> GetByIdAsync(string id);
}
