using AutoMapper;
using Domain.Models;
using Domain.DTOs;
using Application.DTOs.StudentDtos;
using Application.DTOs.UserDtos;

namespace Application.Mappers.MappingProfiles;

public class StudentMappingProfiles : Profile
{
    public StudentMappingProfiles()
    {
        // GetStudentByIdInfraDto to GetStudentByIdAppDto
        CreateMap<GetStudentByIdInfraDto, GetStudentByIdAppDto>()
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // CreateStudentAppDto to CreateUserAppDto
        CreateMap<CreateStudentAppDto, CreateUserAppDto>();

        // CreateStudentAppDto to Student entity (with userId parameter)
        CreateMap<CreateStudentAppDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.JoinDate))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => Random.Shared.Next(100000, 200000).ToString()))
            .ForMember(dest => dest.StudentSubjects, opt => opt.MapFrom(src => 
                src.CourseIds != null ? src.CourseIds.Select(id => new StudentSubject { SubjectId = id }).ToList() 
                : new List<StudentSubject>()))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.GeneratedExams, opt => opt.Ignore());
    }
}