using System.Runtime.Versioning;

namespace BlazorBingo.Components;

[SupportedOSPlatform("browser")]
public partial class BingoCard : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLowerInvariant())
        {
            case "pick":
                Console.WriteLine($"pick: {data}");
                flashboard!.Add(data[0], Convert.ToInt32(data.Substring(2)));
                Interop.Speak(data.Replace("-", " - "));
                break;
            case "restart":
                ClearCard();
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}