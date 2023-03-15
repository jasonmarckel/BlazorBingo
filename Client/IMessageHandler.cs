namespace BlazorBingo.Client;

public interface IMessageHandler
{
    Task HandleMessage(string messageType, string data);
}
