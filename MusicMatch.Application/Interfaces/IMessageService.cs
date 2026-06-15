namespace MusicMatch.Application.Interfaces;

public interface IMessageService
{
    Task PublishAsync<T>(string eventName, T message);
}