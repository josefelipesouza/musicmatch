using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MusicMatch.Infrastructure.Messaging;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly RabbitMqConsumer _consumer;

    public RabbitMqConsumerService(IConfiguration config)
    {
        _consumer = new RabbitMqConsumer(
            hostName: config["RabbitMQ:Host"] ?? "localhost",
            smtpHost: config["Smtp:Host"] ?? "smtp.gmail.com",
            smtpPort: int.Parse(config["Smtp:Port"] ?? "587"),
            smtpUser: config["Smtp:User"] ?? "",
            smtpPassword: config["Smtp:Password"] ?? "",
            fromEmail: config["Smtp:FromEmail"] ?? ""
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartConsumingAsync(stoppingToken);
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}