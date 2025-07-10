using Application.DTOs.UserDtos;
using Domain.DTOs.UserDtos;

namespace Application.Mappers.UserMappers;

public static class CreateUserDtoMappers
{
    // from service layer to infrastructure layer
    public static CreateUserInfraDto ToUser(this CreateUserServiceDto infraDto)
    {
        return new CreateUserInfraDto
        {
            FirstName = infraDto.FirstName,
            LastName = infraDto.LastName,
            Email = infraDto.Email,
            Birthdate = infraDto.Birthdate,
            Gender = infraDto.Gender,
            Password = infraDto.Password
        }; 
    }

    public static UserServiceDto ToUserServiceDto(this CreateUserServiceDto createDto, string userId)
    {
        return new UserServiceDto
        {
            Id = userId,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            Birthdate = createDto.Birthdate,
            Gender = createDto.Gender
        };
    }
}
