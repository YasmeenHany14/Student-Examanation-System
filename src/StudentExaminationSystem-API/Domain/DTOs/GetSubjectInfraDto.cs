﻿namespace Domain.DTOs;

public class GetSubjectInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool? HasConfiguration { get; set; }
}
