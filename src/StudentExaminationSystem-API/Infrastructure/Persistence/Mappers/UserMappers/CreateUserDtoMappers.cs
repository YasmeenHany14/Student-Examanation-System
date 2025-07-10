using Domain.DTOs.UserDtos;
using Domain.Models;

namespace Infrastructure.Persistence.Mappers.UserMappers;

public static class CreateUserDtoMappers
{
    public static User ToUser(this CreateUserInfraDto infraDto)
    {
        return new User
        {
            FirstName = infraDto.FirstName,
            LastName = infraDto.LastName,
            Email = infraDto.Email,
            Birthdate = infraDto.Birthdate,
            Gender = infraDto.Gender,
        };
    }
}