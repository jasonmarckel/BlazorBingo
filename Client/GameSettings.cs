﻿namespace BlazorBingo.Client;

// In-memory state container service
// https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-7.0&pivots=webassembly#in-memory-state-container-service-wasm

public class GameSettings
{
    private Blazored.LocalStorage.ILocalStorageService localStorage;

    private class Settings
    {
        public string playerName { get; set; } = string.Empty;
        public string dauber { get; set; } = "Random";
        public string callerVoice { get; set; } = string.Empty;
        public string cardTheme { get; set; } = "blue";
        public string selectedLanguage { get; set; } = string.Empty;
        public string selectedPattern { get; set; } = GamePatterns.Default; // traditional
        public bool isMuted { get; set; }
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
    public bool IsMuted
    {
        get => settings.isMuted;
        set
        {
            settings.isMuted = value;
            _ = SaveAsync();
            NotifyStateChanged();
        }
    }
    public string SelectedLanguage
    {
        get => settings.selectedLanguage;
        set
        {
            settings.selectedLanguage = value;
            _ = SaveAsync();
            NotifyStateChanged();
        }
    }
    public string SelectedPattern
    {
        get => settings.selectedPattern;
        set
        {
            settings.selectedPattern = value;
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
        settings = await localStorage.GetItemAsync<Settings>("Settings") ?? new Settings();
    }

    public async Task SaveAsync()
    {
        await localStorage.SetItemAsync<Settings>("Settings", settings);
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
