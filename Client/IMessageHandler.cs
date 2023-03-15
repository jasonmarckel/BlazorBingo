namespace BlazorBingo;

public interface IMessageHandler
{
    Task HandleMessage(string messageType, string data);
}
