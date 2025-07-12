using Application.DTOs.UserDtos;

namespace Application.Mappers.StudentMappers;
using Application.DTOs.StudentDtos;
using Domain.Enums;
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
            StudentId = Random.Shared.Next(100000, 200000).ToString()
        };
    }

    public static GetStudentAppDto ToGetStudentAppDto(this CreateStudentAppDto studentDto, string userId)
    {
        return new GetStudentAppDto()
        {
            Id = userId,
            Name = studentDto.FirstName + " " + studentDto.LastName,
            Email = studentDto.Email,
            Age = DateTime.Now.Year - studentDto.Birthdate.Year,
            JoinDate = studentDto.JoinDate
        };
    }
}
