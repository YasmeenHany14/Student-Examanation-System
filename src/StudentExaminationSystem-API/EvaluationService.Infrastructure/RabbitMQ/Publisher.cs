using System.Text;
using EvaluationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EvaluationService.Infrastructure.RabbitMQ;

public class Publisher(ILogger<Publisher> logger) : IPublisher
{
    private readonly string _evaluationQueueName = "evaluation_queue";
    public async Task PublishEvaluationAsync(int examId, int totalScore)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _evaluationQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        var message = JsonConvert.SerializeObject(new
        {
            ExamId = examId,
            TotalScore = totalScore
        });
        
        var body = Encoding.UTF8.GetBytes(message);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _evaluationQueueName,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true }, // message will be saved to disk
            body: body
        );
            
        logger.LogInformation("Published evaluation result for exam ID: {ExamId} with score: {TotalScore}", examId, totalScore);
    }
}
