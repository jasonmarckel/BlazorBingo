namespace BlazorBingo.Pages;

public partial class HostScreen : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLower())
        {
            case "connected":
                players.Add(data); // playerName
                break;
            case "disconnected":
                players.Remove(data); // playerName
                break;
            case "bingo":
                notificationMessage = $"We have a winner! {data} called BINGO!";
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}
