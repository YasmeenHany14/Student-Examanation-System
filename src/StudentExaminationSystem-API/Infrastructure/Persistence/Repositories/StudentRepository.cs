using Domain.DTOs.CommonDtos;
using Domain.DTOs.StudentDtos;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class StudentRepository(DataContext context)
    : BaseRepository<Student>(context), IStudentRepository
{
    public async Task<PagedList<GetStudentByIdInfraDto>> GetAllAsync(StudentResourceParameters resourceParameters)
    {
        IQueryable<Student> collection = context.Students
            .Include(s => s.User)
            .AsNoTracking();
        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
        {
            var searchQuery = resourceParameters.SearchQuery.Trim().ToLower();
            collection = collection.Where(o => o.User.FirstName.ToLower().Contains(searchQuery));
        }
        if (!string.IsNullOrWhiteSpace(resourceParameters.FirstName))
        {
            var firstName = resourceParameters.FirstName.Trim().ToLower();
            collection = collection.Where(o => o.User.FirstName.ToLower().Equals(firstName));
        }
        
        if (!string.IsNullOrWhiteSpace(resourceParameters.LastName))
        {
            var lastName = resourceParameters.LastName.Trim().ToLower();
            collection = collection.Where(o => o.User.LastName.ToLower().Equals(lastName));
        }
             
        var projectedCollection = collection.Select(s => new GetStudentByIdInfraDto
        {
            Id = s.UserId,
            Name = s.User.FirstName + " " + s.User.LastName,
            IsActive = s.User.IsActive
        });
        
        // var sortedList = sortHelper.ApplySort(collection, resourceParameters.OrderBy);
        var createdCollection = await CreateAsync(projectedCollection, resourceParameters.PageNumber, resourceParameters.PageSize);
        return createdCollection;
    }

    public async Task<GetStudentByIdInfraDto?> GetByIdAsync(string id)
    {
        return await context.Students
            .Select(s => new GetStudentByIdInfraDto
            {
                Id = s.UserId,
                Name = s.User.FirstName + " " + s.User.LastName,
                Birthdate = s.User.Birthdate,
                JoinDate = s.EnrollmentDate,
                IsActive = s.User.IsActive,
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
