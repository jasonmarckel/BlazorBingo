using System.Runtime.Versioning;

namespace BlazorBingo.Pages;

[SupportedOSPlatform("browser")]
public partial class PlayScreen : IMessageHandler
{
    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLower())
        {
            case "pick":
                Console.WriteLine($"pick: {data}");
                flashboard!.Add(Convert.ToInt32(data.Substring(2)));
                gameStarted = true;
                if (!isMuted) { Interop.Speak(data.Replace("-", ", "), settings.CallerVoice, settings.SelectedLanguage); }
                break;
            case "restart":
                ClearCard();
                gameStarted= false;
                break;
        }
        this.StateHasChanged(); // force the UI to refresh
    }
}