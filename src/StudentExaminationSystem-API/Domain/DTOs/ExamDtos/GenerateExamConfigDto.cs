namespace Domain.DTOs.ExamDtos;

public class GenerateExamConfigDto : BaseDto
{
    public Dictionary<int, int> QuestionCounts { get; set; } = new Dictionary<int, int>();

    public GenerateExamConfigDto(int totalQuestions, Dictionary<int, int> percentages)
    {
        var remainingQuestions = totalQuestions;
        var difficultyKeys = percentages.Keys.OrderBy(k => k).ToList();
        
        foreach (var difficultyId in difficultyKeys)
        {
            var percentage = percentages[difficultyId];
            var questionCount = (totalQuestions * percentage) / 100;
            QuestionCounts[difficultyId] = questionCount;
            remainingQuestions -= questionCount;
        }
        
        var index = 0;
        while (remainingQuestions > 0 && index < difficultyKeys.Count)
        {
            var difficultyId = difficultyKeys[index];
            QuestionCounts[difficultyId]++;
            remainingQuestions--;
            index++;
            
            // Reset if we are not done yet
            if (index >= difficultyKeys.Count)
                index = 0;
        }
    }
}
