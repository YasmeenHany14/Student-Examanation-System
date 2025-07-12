using Domain.DTOs.CommonDtos;
using Domain.DTOs.StudentDtos;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class StudentRepository(DataContext context)
    : BaseRepository<Student>(context), IStudentRepository
{
    public async Task<GetStudentByIdInfraDto?> GetByIdAsync(string id)
    {
        return await context.Students
            .Select(s => new GetStudentByIdInfraDto
            {
                Id = s.UserId,
                Name = s.User.FirstName + " " + s.User.LastName,
                Birthdate = s.User.Birthdate,
                JoinDate = s.EnrollmentDate,
                Courses = s.StudentSubjects
                    .Where(ss => s.Id == ss.StudentId)
                    .Select(ss => new DropdownInfraDto
                    {
                        Id = ss.SubjectId,
                        Name = ss.Subject.Name
                    })
                    .ToList()
            }).FirstOrDefaultAsync(s => s.Id == id);
    }
}
