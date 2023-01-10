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
                flashboard!.Add(Convert.ToInt32(data.Substring(2)));
                if (!isMuted) { Interop.Speak(data.Replace("-", " - ")); }
                break;
            case "restart":
                ClearCard();
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}