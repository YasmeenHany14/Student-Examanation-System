using Application.DTOs.UserDtos;
using Domain.Models;

namespace Application.Mappers.UserMappers;

public static class CreateUserDtoMappers
{
    // from service layer to infrastructure layer
    public static User ToUser(this CreateUserAppDto infraDto)
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
