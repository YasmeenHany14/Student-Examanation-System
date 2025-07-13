using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SubjectRepository(DataContext context) : BaseRepository<Subject>(context), ISubjectRepository
{
    public async Task<bool> CheckSubjectsExists(IEnumerable<int> subjectIds)
    {
        return await context.Subjects
            .Where(s => subjectIds.Contains(s.Id))
            .CountAsync() == subjectIds.Count();
    }
}
