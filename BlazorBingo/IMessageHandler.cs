namespace BlazorBingo;

public interface IMessageHandler
{
    void HandleMessage(string messageType, string data);
}
