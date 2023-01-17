using System.Runtime.Versioning;

namespace BlazorBingo.Pages;

[SupportedOSPlatform("browser")]
public partial class PlayScreen : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLowerInvariant())
        {
            case "pick":
                Console.WriteLine($"pick: {data}");
                flashboard!.Add(Convert.ToInt32(data.Substring(2)));
                if (!isMuted) { Interop.Speak(data.Replace("-", " - "), settings.CallerVoice, settings.SelectedLanguage); }
                break;
            case "restart":
                ClearCard();
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}