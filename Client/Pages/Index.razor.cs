using System.Runtime.Versioning;

namespace BlazorBingo.Client.Pages;

[SupportedOSPlatform("browser")]
public partial class Index
{
    protected string gameCode = string.Empty;
    protected string playerName = string.Empty;
    protected bool canJoin;

    protected override async Task OnInitializedAsync()
    {
        await settings.LoadAsync();
        playerName = settings.PlayerName;
        if (string.IsNullOrEmpty(settings!.SelectedLanguage))
        {
            settings.SelectedLanguage = await Interop.GetLanguage();
        }
#if DEBUG
        gameCode = "0000";
        RefreshCanJoin();
#endif
        await Interop.ReleaseWakeLock();
    }

    private void RefreshCanJoin()
    {
        canJoin = !string.IsNullOrWhiteSpace(playerName) && !string.IsNullOrWhiteSpace(gameCode) && gameCode.Length == 4;
    }

    protected void OnGameCodeInput(string value)
    {
        gameCode = value;
        RefreshCanJoin();
    }

    protected async Task Host()
    {
        settings.PlayerName = playerName;
        await Interop.PrimeSpeechSynthesis(); // prime speechSynthesis for iOS
        Navigation.NavigateTo("./host");
    }

    protected async Task Join()
    {
        settings.PlayerName = playerName;
        await Interop.PrimeSpeechSynthesis(); // prime speechSynthesis for iOS
        Navigation.NavigateTo($"./play/{gameCode.ToUpper()}");
    }
}