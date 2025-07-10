using Application.DTOs.UserDtos;
using Domain.DTOs.UserDtos;

namespace Application.Mappers.UserMappers;

public static class UserDtoMappers
{
    // from service layer to infrastructure layer and vice versa
    public static UserServiceDto ToUserServiceDto(this UserInfraDto user)
    {
        return new UserServiceDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthdate = user.Birthdate
        };
    }


    public static UserInfraDto ToUserInfraDto(this UserServiceDto infraDto)
    {
        return new UserInfraDto
        {
            FirstName = infraDto.FirstName,
            LastName = infraDto.LastName,
            Birthdate = infraDto.Birthdate,
        };
    }
}