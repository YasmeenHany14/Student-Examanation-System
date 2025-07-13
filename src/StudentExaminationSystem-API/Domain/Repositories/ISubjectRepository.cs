namespace Domain.Repositories;

public interface ISubjectRepository
{
    Task<bool> CheckSubjectsExists(IEnumerable<int> subjectIds);
}
