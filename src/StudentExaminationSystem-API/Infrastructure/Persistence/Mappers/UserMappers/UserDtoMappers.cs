using Domain.DTOs.UserDtos;
using Domain.Models;

namespace Infrastructure.Persistence.Mappers.UserMappers;

public static class UserDtoMappers
{
    public static UserInfraDto ToUserDto(this User user)
    {
        return new UserInfraDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthdate = user.Birthdate
        };
    }


    public static User ToUser(this UserInfraDto infraDto)
    {
        return new User
        {
            FirstName = infraDto.FirstName,
            LastName = infraDto.LastName,
            Birthdate = infraDto.Birthdate,
        };
    }
}