using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class StudentRepository(DataContext context)
    : BaseRepository<Student>(context), IStudentRepository
{
    
}
