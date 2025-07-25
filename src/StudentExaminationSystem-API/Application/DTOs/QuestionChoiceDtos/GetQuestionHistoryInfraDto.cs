﻿using Domain.DTOs;
namespace Application.DTOs.QuestionChoiceDtos;

public class GetQuestionHistoryAppDto : AppBaseDto
{
    public string Question { get; set; } = string.Empty;
    public IEnumerable<GetQuestionChoiceHistoryAppDto> Choices { get; set; } = new List<GetQuestionChoiceHistoryAppDto>();
}
