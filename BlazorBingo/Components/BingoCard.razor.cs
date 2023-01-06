namespace BlazorBingo.Components;

public partial class BingoCard : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLowerInvariant())
        {
            case "pick":
                Console.WriteLine($"pick: {data}");
                calledNumbers.Add(data);
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}