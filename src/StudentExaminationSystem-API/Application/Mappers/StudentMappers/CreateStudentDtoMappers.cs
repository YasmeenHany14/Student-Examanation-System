using Application.DTOs.UserDtos;

namespace Application.Mappers.StudentMappers;
using DTOs.StudentDtos;
using Domain.Models;

public static class CreateStudentDtoMappers
{
    public static CreateUserAppDto ToCreateUserAppDto(this CreateStudentAppDto createStudentAppDto)
    {
        return new CreateUserAppDto
        {
            FirstName = createStudentAppDto.FirstName,
            LastName = createStudentAppDto.LastName,
            Email = createStudentAppDto.Email,
            Password = createStudentAppDto.Password,
            ConfirmPassword = createStudentAppDto.ConfirmPassword,
            Birthdate = createStudentAppDto.Birthdate,
            Gender = createStudentAppDto.Gender
        };
    }

    //TODO: should i really expose entity models here? not doing so will prevent me from using generic repo
    public static Student ToEntity(
        this CreateStudentAppDto createStudentAppDto,
        string userId)
    {
        return new Student()
        {
            UserId = userId,
            EnrollmentDate = createStudentAppDto.JoinDate,
            StudentId = Random.Shared.Next(100000, 200000).ToString(),
            StudentSubjects = createStudentAppDto.CourseIds?.Select(id => new StudentSubject()
            {
                SubjectId = id
            }).ToList() ?? new List<StudentSubject>()
        };
    }
}
