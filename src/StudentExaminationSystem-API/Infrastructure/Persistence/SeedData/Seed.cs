using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.SeedData;

public class Seed(DataContext context)
{
    public void SeedDataContext(UserManager<User> userManager)
    {
        if (!context.Users.Any())
        {
            var currentDate = DateTimeOffset.UtcNow.DateTime;

            // 1. Create Users
            var user1 = new User { UserName = "alice.smith@example.com", Email = "alice.smith@example.com", FirstName = "Alice", LastName = "Smith", Birthdate = new DateOnly(2000, 1, 1), Gender = Domain.Enums.Gender.Female };
            var user2 = new User { UserName = "bob.jones@example.com", Email = "bob.jones@example.com", FirstName = "Bob", LastName = "Jones", Birthdate = new DateOnly(1999, 5, 15), Gender = Domain.Enums.Gender.Male };
            var user3 = new User { UserName = "carol.lee@example.com", Email = "carol.lee@example.com", FirstName = "Carol", LastName = "Lee", Birthdate = new DateOnly(2001, 3, 10), Gender = Domain.Enums.Gender.Female };

            // Create users and await creation
            userManager.CreateAsync(user1, "Password123!").Wait();
            userManager.CreateAsync(user2, "Password123!").Wait();
            userManager.CreateAsync(user3, "Password123!").Wait();
            context.SaveChanges();

            // Reload users from DB to get their generated Ids
            var dbUser1 = context.Users.First(u => u.UserName == user1.UserName);
            var dbUser2 = context.Users.First(u => u.UserName == user2.UserName);
            var dbUser3 = context.Users.First(u => u.UserName == user3.UserName);

            // 2. Create Students with correct UserId
            var student1 = new Student { UserId = dbUser1.Id, StudentId = "S1001", EnrollmentDate = currentDate };
            var student2 = new Student { UserId = dbUser2.Id, StudentId = "S1002", EnrollmentDate = currentDate };
            context.Students.AddRange(student1, student2);

            // 3. Create Subjects
            var math = new Subject { Name = "Mathematics", Code = "MATH" };
            var science = new Subject { Name = "Science", Code = "SCI1" };
            context.Subjects.AddRange(math, science);
            
            context.SaveChanges();

            // 5. Create Questions and Choices (10 questions, 4 choices each)
            var questions = new List<Question>
            {
                new Question { SubjectId = math.Id, Content = "What is 2+2?", Difficulty = Difficulty.Easy, IsActive = true },
                new Question { SubjectId = science.Id, Content = "What planet is known as the Red Planet?", Difficulty = Difficulty.Easy, IsActive = true },
                new Question { SubjectId = math.Id, Content = "What is 10/2?", Difficulty = Difficulty.Medium, IsActive = true },
                new Question { SubjectId = science.Id, Content = "What is H2O?", Difficulty = Difficulty.Easy, IsActive = true },
                new Question { SubjectId = math.Id, Content = "What is 3*3?", Difficulty = Difficulty.Medium, IsActive = true },
                new Question { SubjectId = science.Id, Content = "What is the boiling point of water?", Difficulty = Difficulty.Hard, IsActive = true },
                new Question { SubjectId = math.Id, Content = "What is the derivative of x^2?", Difficulty = Difficulty.Hard, IsActive = true },
                new Question { SubjectId = science.Id, Content = "What is the chemical symbol for gold?", Difficulty = Difficulty.Medium, IsActive = true },
                new Question { SubjectId = math.Id, Content = "What is 7+8?", Difficulty = Difficulty.Easy, IsActive = true },
                new Question { SubjectId = science.Id, Content = "What is the largest planet?", Difficulty = Difficulty.Medium, IsActive = true }
            };
            context.Questions.AddRange(questions);
            context.SaveChanges();

            var choices = new List<QuestionChoice>
            {
                // Q1
                new QuestionChoice { QuestionId = questions[0].Id, Content = "4", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[0].Id, Content = "5", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[0].Id, Content = "3", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[0].Id, Content = "6", IsCorrect = false },
                // Q2
                new QuestionChoice { QuestionId = questions[1].Id, Content = "Mars", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[1].Id, Content = "Venus", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[1].Id, Content = "Jupiter", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[1].Id, Content = "Saturn", IsCorrect = false },
                // Q3
                new QuestionChoice { QuestionId = questions[2].Id, Content = "5", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[2].Id, Content = "10", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[2].Id, Content = "2", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[2].Id, Content = "7", IsCorrect = false },
                // Q4
                new QuestionChoice { QuestionId = questions[3].Id, Content = "Water", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[3].Id, Content = "Oxygen", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[3].Id, Content = "Hydrogen", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[3].Id, Content = "Carbon", IsCorrect = false },
                // Q5
                new QuestionChoice { QuestionId = questions[4].Id, Content = "6", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[4].Id, Content = "9", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[4].Id, Content = "12", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[4].Id, Content = "3", IsCorrect = false },
                // Q6
                new QuestionChoice { QuestionId = questions[5].Id, Content = "100°C", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[5].Id, Content = "0°C", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[5].Id, Content = "50°C", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[5].Id, Content = "200°C", IsCorrect = false },
                // Q7
                new QuestionChoice { QuestionId = questions[6].Id, Content = "2x", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[6].Id, Content = "x", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[6].Id, Content = "x^2", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[6].Id, Content = "1", IsCorrect = false },
                // Q8
                new QuestionChoice { QuestionId = questions[7].Id, Content = "Au", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[7].Id, Content = "Ag", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[7].Id, Content = "Fe", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[7].Id, Content = "Cu", IsCorrect = false },
                // Q9
                new QuestionChoice { QuestionId = questions[8].Id, Content = "15", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[8].Id, Content = "14", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[8].Id, Content = "13", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[8].Id, Content = "16", IsCorrect = false },
                // Q10
                new QuestionChoice { QuestionId = questions[9].Id, Content = "Jupiter", IsCorrect = true },
                new QuestionChoice { QuestionId = questions[9].Id, Content = "Earth", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[9].Id, Content = "Mars", IsCorrect = false },
                new QuestionChoice { QuestionId = questions[9].Id, Content = "Venus", IsCorrect = false }
            };
            context.QuestionChoices.AddRange(choices);
            context.SaveChanges();

            // 6. Create GeneratedExams
            var exam1 = new GeneratedExam { StudentId = student1.Id, SubjectId = math.Id, SubmittedAt = currentDate, ExamStatus = ExamStatus.Completed};
            var exam2 = new GeneratedExam { StudentId = student2.Id, SubjectId = science.Id, SubmittedAt = currentDate, ExamStatus = ExamStatus.Completed, StudentScore = 0 };
            context.GeneratedExams.AddRange(exam1, exam2);
            context.SaveChanges();

            // 7. Create AnswerHistories for 10 questions per exam
            var answerHistories = new List<AnswerHistory>();
            for (int i = 0; i < 10; i++)
            {
                var correctChoice = choices.First(qc => qc.QuestionId == questions[i].Id && qc.IsCorrect);
                var incorrectChoice = choices.First(qc => qc.QuestionId == questions[i].Id && !qc.IsCorrect);
                answerHistories.Add(new AnswerHistory
                {
                    GeneratedExamId = exam1.Id,
                    QuestionId = questions[i].Id,
                    QuestionChoiceId = correctChoice.Id,
                    IsCorrect = true,
                    DisplayOrder = i + 1
                });
                answerHistories.Add(new AnswerHistory
                {
                    GeneratedExamId = exam2.Id,
                    QuestionId = questions[i].Id,
                    QuestionChoiceId = incorrectChoice.Id,
                    IsCorrect = false,
                    DisplayOrder = i + 1
                });
            }
            context.AnswerHistories.AddRange(answerHistories);
            context.SaveChanges();

            // 8. Seed configuration profiles
            var difficultyProfile = new DifficultyProfile { Name = "Standard Profile", EasyPercentage = 0, MediumPercentage = 50, HardPercentage = 50};
            context.DifficultyProfiles.Add(difficultyProfile);
            context.SaveChanges();
            var subjectExamConfig = new SubjectExamConfig { SubjectId = math.Id, TotalQuestions = 10, DurationMinutes = 60, DifficultyProfileId = difficultyProfile.Id };
            context.SubjectExamConfigs.Add(subjectExamConfig);
            
            context.SaveChanges();
        }
    }
}
