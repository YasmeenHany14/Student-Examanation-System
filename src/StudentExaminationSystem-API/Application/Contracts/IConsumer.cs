namespace Application.Contracts;

public interface IConsumer
{
    Task ConsumeAsync();
}