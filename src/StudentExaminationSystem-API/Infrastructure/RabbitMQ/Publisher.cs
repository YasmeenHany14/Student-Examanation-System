using System.Text;
using Domain.DTOs.ExamDtos;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.RabbitMQ;

public class Publisher(ILogger<Publisher> logger) : IPublisher
{
    private readonly string _queueName = "exam_queue";
    public async Task PublishExamAsync(
        IEnumerable<CorrectAnswersInfraDto> answersForExam,
        int examId,
        IEnumerable<EvaluateQuestionDto> evaluationQuestions)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        var message = JsonConvert.SerializeObject(new
        {
            ExamId = examId,
            CorrectAnswers = answersForExam,
            EvaluateQuestions = evaluationQuestions
        });
        
        var body = Encoding.UTF8.GetBytes(message);
        
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: body
        );
        
        logger.LogInformation("Published exam with ID {ExamId} to queue {QueueName}", examId, _queueName);
    }
}
