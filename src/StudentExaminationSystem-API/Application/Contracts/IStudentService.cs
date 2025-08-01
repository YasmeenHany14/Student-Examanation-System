﻿using Application.Common.ErrorAndResults;
using Application.DTOs.StudentDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface IStudentService
{
    Task<Result<PagedList<GetStudentByIdAppDto>>> GetAllAsync(StudentResourceParameters resourceParameters);
    Task<Result<string>> AddAsync(CreateStudentAppDto studentAppDto);
    Task<Result<GetStudentByIdAppDto>> GetByIdAsync(string id);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> AddStudentSubjectAsync(string studentId, int subjectId);
}
