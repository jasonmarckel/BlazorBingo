using System.Runtime.Versioning;

namespace BlazorBingo.Client.Pages;

[SupportedOSPlatform("browser")]
public partial class Index
{
    protected string gameCode = string.Empty;
    protected string playerName = string.Empty;
    protected bool canJoin;
    protected const string BINGO_KEY_CHARS = "BCDFGHJKLMNPQRSTVWXYZ";
    protected bool isIOS;
    protected bool isInstallBannerVisible;
    protected string appVersion = "BUILD_DATETIME"; // value will be replaced by step in main.yml

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
        appVersion = DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
        RefreshCanJoin();
#endif
        isIOS = await Interop.IsIOS();
        var isInStandaloneMode = await Interop.IsInStandaloneMode();
        var isInstalled = await Interop.IsInstalled();
        isInstallBannerVisible = !isInstalled || (isIOS && !isInStandaloneMode);
        await Interop.ReleaseWakeLock();
    }

    private void RefreshCanJoin()
    {
        canJoin = !string.IsNullOrWhiteSpace(playerName) && !string.IsNullOrWhiteSpace(gameCode) && gameCode.Length == 4;
    }

    protected void OnGameCodeInput(string? value)
    {
        gameCode = value ?? string.Empty;
        RefreshCanJoin();
    }

    protected async Task Host()
    {
        if (string.IsNullOrWhiteSpace(gameCode))
        {
            gameCode = Utility.GenerateKey(4, BINGO_KEY_CHARS);
        }
        settings.PlayerName = playerName;
        await Interop.PrimeSpeechSynthesis(); // prime speechSynthesis for iOS
        Navigation.NavigateTo($"./host/{gameCode}");
    }

    protected async Task Join()
    {
        settings.PlayerName = playerName;
        await Interop.PrimeSpeechSynthesis(); // prime speechSynthesis for iOS
        Navigation.NavigateTo($"./play/{gameCode.ToUpper()}");
    }

    protected async void ShareGameUrl()
    {
        await Interop.ShareUrl("BlazorBingo", "Play a game of bingo with your friends!", @"https://jasonmarckel.github.io/BlazorBingo/");
    }

    protected async void InstallPWA()
    {
        isInstallBannerVisible = false;
        await Interop.InstallPWA();
    }
}