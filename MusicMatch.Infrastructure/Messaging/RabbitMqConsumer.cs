using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using MusicMatch.Application.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MusicMatch.Infrastructure.Messaging;

public class RabbitMqConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    private const string QueueName = "musicmatch-events";

    public RabbitMqConsumer(string hostName, string smtpHost, int smtpPort,
        string smtpUser, string smtpPassword, string fromEmail)
    {
        _fromEmail = fromEmail;

        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        ).GetAwaiter().GetResult();

        _smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPassword),
            EnableSsl = true,
        };
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());

                var envelope = JsonDocument.Parse(body).RootElement;
                var eventName = envelope.GetProperty("Event").GetString();
                var data = envelope.GetProperty("Data");

                if (eventName == "artista.notificado")
                {
                    var dto = JsonSerializer.Deserialize<NotificarArtistaDto>(
                        data.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (dto is not null)
                        await EnviarEmailAsync(dto);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Consumer] Erro ao processar mensagem: {ex.Message}");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer
        );

        Console.WriteLine("[Consumer] Aguardando mensagens...");

        // Mantém o consumer vivo
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private async Task EnviarEmailAsync(NotificarArtistaDto dto)
    {
        var subject = $"🎵 Nova proposta de evento — {dto.EventoTipo}";

        var nomeArtista = string.IsNullOrWhiteSpace(dto.ArtistaRazaoSocial)
            ? dto.ArtistaNome
            : $"{dto.ArtistaNome} ({dto.ArtistaRazaoSocial})";

        var celularContratante = string.IsNullOrWhiteSpace(dto.ContratanteCelular2)
            ? dto.ContratanteCelular1
            : $"{dto.ContratanteCelular1} / {dto.ContratanteCelular2}";

        var body = $"""
            Olá, {nomeArtista}!

            Você recebeu uma proposta de contratação para um evento.

            📅 Data: {dto.EventoData}
            🕐 Horário: {dto.EventoHorario}
            📍 Local: {dto.EventoLocalizacao}
            🎵 Formato: {dto.FormatoShow}
            💰 Cache por hora: até R$ {dto.BaseCacheHoraAte:F2}

            ── Contratante ──
            Nome: {dto.ContratanteNome}
            Email: {dto.ContratanteEmail}
            Telefone: {celularContratante}

            Acesse o MusicMatch para responder esta proposta.

            Atenciosamente,
            Equipe MusicMatch
            """;

        var mail = new MailMessage
        {
            From = new MailAddress(_fromEmail, "MusicMatch"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };

        mail.To.Add(dto.ArtistaEmail);

        await _smtpClient.SendMailAsync(mail);

        Console.WriteLine($"[Consumer] Email enviado para {dto.ArtistaEmail}");
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}