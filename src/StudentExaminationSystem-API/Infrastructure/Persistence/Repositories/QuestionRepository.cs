using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class QuestionRepository(DataContext context) : BaseRepository<Question>(context), IQuestionRepository
{
}