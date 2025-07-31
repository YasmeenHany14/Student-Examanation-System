using System.Text;
using Application.Contracts;
using Application.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Application.Services;

public class Consumer(
    ILogger<Consumer> logger,
    IServiceScopeFactory serviceScopeFactory) : IConsumer
{
    private readonly string _queueName = "evaluation_queue";

    public async Task ConsumeAsync()
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

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            using var scope = serviceScopeFactory.CreateScope();
            var messageProcessingService = scope.ServiceProvider.GetRequiredService<IMessageProcessingService>();
            
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = System.Text.Json.JsonSerializer.Deserialize<ExamEvaluationDto>(message);
                logger.LogInformation("Received message for exam ID: {ExamId}", data.ExamId);
                await messageProcessingService.ProcessExamEvaluationMessageAsync(data);
                await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");
                await channel.BasicNackAsync(
                    deliveryTag: eventArgs.DeliveryTag,
                    multiple: false,
                    requeue: false
                );
            }
        };
        
        await channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: consumer
        );
        
        logger.LogInformation("Started consuming messages from queue: {QueueName}", _queueName);
        
        await Task.Delay(-1);
    }
}