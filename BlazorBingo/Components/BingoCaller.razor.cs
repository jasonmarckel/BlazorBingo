namespace BlazorBingo.Components;

public partial class BingoCaller : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLowerInvariant())
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
