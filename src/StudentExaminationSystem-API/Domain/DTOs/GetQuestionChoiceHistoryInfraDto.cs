﻿namespace Domain.DTOs;

public class GetQuestionChoiceHistoryInfraDto : BaseDto
{
    public int ChoiceId { get; set; }
    public string ChoiceText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsSelected { get; set; }
}
