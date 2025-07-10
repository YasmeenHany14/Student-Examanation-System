namespace Application.Common.ErrorAndResults;

public interface IResult
{
    bool IsSuccess { get; }
    Error Error { get; }
}
