namespace BlazorBingo;

// In-memory state container service
// https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-7.0&pivots=webassembly#in-memory-state-container-service-wasm

public class GameSettings
{
    private Blazored.LocalStorage.ILocalStorageService localStorage;

    private class Settings
    {
        public string playerName { get; set; } = string.Empty;
        public string dauber { get; set; } = "random";
        public string callerVoice { get; set; } = string.Empty;
        public string cardTheme { get; set; } = string.Empty;
    }

    private Settings settings = new();

    public string PlayerName 
    {
        get => settings.playerName;
        set
        {
            settings.playerName = value;
            _ = SaveAsync();
            NotifyStateChanged();
        } 
    }
    public string Dauber 
    {
        get => settings.dauber; 
        set
        {
            settings.dauber = value;
            _ = SaveAsync();
            NotifyStateChanged();
        }
    }
    public string CallerVoice 
    {
        get => settings.callerVoice;
        set
        {
            settings.callerVoice = value;
            _ = SaveAsync();
            NotifyStateChanged();
        }
    }
    public string CardTheme 
    {
        get => settings.cardTheme;
        set
        {
            settings.cardTheme = value;
            _ = SaveAsync();
            NotifyStateChanged();
        }
    }

    public GameSettings(Blazored.LocalStorage.ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public async Task LoadAsync()
    {
        settings = await localStorage.GetItemAsync<Settings>("Settings");
    }

    public async Task SaveAsync()
    {
        await localStorage.SetItemAsync<Settings>("Settings", settings);
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
