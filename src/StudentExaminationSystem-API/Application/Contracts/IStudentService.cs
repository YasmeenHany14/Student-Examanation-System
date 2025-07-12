using Application.Common.ErrorAndResults;
using Application.DTOs.StudentDtos;

namespace Application.Contracts;

public interface IStudentService
{
    Task<Result<GetStudentAppDto>> AddAsync(CreateStudentAppDto studentAppDto);
    Task<Result<GetStudentByIdAppDto>> GetByIdAsync(string id);
    Task<Result> DeleteAsync(int id);
}
