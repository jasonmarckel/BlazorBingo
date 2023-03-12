﻿using BlazorBingo.Shared;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Runtime.Versioning;

namespace BlazorBingo.Pages;

[SupportedOSPlatform("browser")]
public partial class PlayScreen : IMessageHandler, IDisposable
{
    protected bool isHost;
    protected bool isMuted;
    protected bool isCalling;
    protected bool showSettings;
    protected bool gameStarted;

    protected Flashboard? flashboard { get; set; }

    protected const int boardSize = 75;

    protected SortedSet<string> players = new();
    protected string notificationMessage = string.Empty;
    protected const string BINGO_KEY_CHARS = "BCDFGHJKLMNPQRSTVWXYZ";

    public void HandleMessage(string messageType, string data)
    {
        switch (messageType.ToLower())
        {
            case "pick":
                Console.WriteLine($"pick: {data}");
                flashboard!.UpdateCalledNumbersCSV(data);
                gameStarted = true;
                if (!isMuted) { Interop.Speak(flashboard!.LastCalled, settings.CallerVoice, settings.SelectedLanguage); }
                break;
            case "restart":
                flashboard!.ClearBoard();
                ClearCard();
                gameStarted = false;
                break;
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

    protected string StamperCssClass
    {
        get
        {
            return settings.Dauber == "random" ? SettingsScreen.Daubers[ThreadSafeRandom.ThisThreadsRandom.Next(1, SettingsScreen.Daubers.Length)].CssName() : settings.Dauber;
        }
    }

    // https://stackoverflow.com/questions/60812587/c-sharp-non-nullable-field-lateinit

    protected readonly Square[,] squares = new Square[5, 5];

    protected void ToggleMute()
    {
        isMuted = !isMuted;
    }

    protected void GenerateNewCard()
    {
        int numberOfValuesPerColumn = boardSize / 5;

        // generate a set of 5 unique integers for each column
        for (int c = 0; c < 5; c++)
        {
            // determine the start number for the column
            int startValue = (c * numberOfValuesPerColumn) + 1;
            // generate the range of numbers and shuffle
            var uniqueValues = new List<int>(Enumerable.Range(startValue, numberOfValuesPerColumn));
            uniqueValues.Shuffle();
            // copy the unique values for the column to the card
            for (int r = 0; r < 5; r++)
            {
                squares[r, c] = new Square() { Value = uniqueValues.ElementAt(r) };
                if (c == 2 && r == 2)
                {
                    squares[r, c].Value = 0; //FREE
                    squares[r, c].StampClass = "stamp solidstar"; //TODO: theme the star
                }
            }
        }
    }

    protected class Square
    {
        public int Value { get; set; }
        public string StampClass { get; set; } = string.Empty;
        public int StampRotation { get; set; }
        public bool IsStamped { get { return !string.IsNullOrWhiteSpace(StampClass); } }
    }

    protected void StampSquare(MouseEventArgs e, int row, int col)
    {
        if (row == 2 && col == 2) { return; }
        var square = squares[row, col];
        square.StampRotation = ThreadSafeRandom.ThisThreadsRandom.Next(-30, 30);
        square.StampClass = square.IsStamped ? string.Empty : "stamp " + StamperCssClass;
    }

    protected void ClearCard()
    {
        for (int c = 0; c < 5; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                if (c == 2 && r == 2) { continue; }
                squares[r, c].StampClass = string.Empty;
            }
        }
    }

    protected void CallBingo()
    {
        var messageType = IsValidCard() ? "bingo" : "falsie";
        Interop.NotifyHost(messageType, settings.PlayerName);
    }

    private bool IsValidCard()
    {
        uint card = 0;
        for (var r = 0; r < 5; r++)
        {
            for (var c = 0; c < 5; c++)
            {
                card <<= 1;
                card |= (squares[r, c].IsStamped && (flashboard!.CalledNumbers.Contains(squares[r, c].Value) || squares[r, c].Value == 0)) ? (uint)1 : (uint)0;
            }
        }
        bool isValid = false;
        foreach (var pattern in SettingsScreen.StraightLinePatterns)
        {
            if ((pattern & card) == pattern) { isValid = true; break; }
        }
        Console.WriteLine($"Is valid Bingo? {isValid}");
        return isValid;
    }

    protected override void OnInitialized()
    {
        GenerateNewCard();
        settings.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        settings.OnChange -= StateHasChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        isHost = Navigation.Uri.Contains("/host", StringComparison.OrdinalIgnoreCase);

        if (isHost)
        {
            GameCode = Utility.GenerateKey(4, BINGO_KEY_CHARS);
#if DEBUG
            GameCode = "0000";
#endif
            await Interop.Host(this, GameCode);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(GameCode)) { Navigation.NavigateTo("./"); return; }
            // https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle?view=aspnetcore-7.0#handle-incomplete-async-actions-at-render
            // https://aryehsilver.co.uk/blazor-attempts-to-render-before-oninitializedasync-method-has-finished/
            await Interop.Connect(this, GameCode!.ToUpper(), settings.PlayerName);
        }

        await Interop.RequestWakeLock();
        await Interop.InitVoices();
    }

    //protected override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    return base.OnAfterRenderAsync(firstRender);
    //}

    // https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-7.0#handleprevent-location-changes
    //private IDisposable? registration;

    //protected override void OnAfterRender(bool firstRender)
    //{
    //    if (firstRender)
    //    {
    //        registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    //    }
    //}

    //private ValueTask OnLocationChanging(LocationChangingContext context)
    //{
    //    return ValueTask.CompletedTask;
    //}

    //public void Dispose()
    //{
    //    registration?.Dispose();
    //}

    protected void ToggleSettings()
    {
        showSettings = !showSettings;
    }

    #region "Host"

    protected async void Pick()
    {        
        isCalling = true;
        flashboard!.Pick();
        notificationMessage = string.Empty;        
        await Interop.Broadcast("pick", flashboard!.CalledNumbersCSV);
        if (!isMuted) { await Interop.Speak(flashboard!.LastCalled, settings.CallerVoice, settings.SelectedLanguage); }
        await Task.Delay(3000); // wait a few seconds before the next pick can be made
        isCalling = false;
        StateHasChanged(); // refresh the UI
    }

    protected async void Copy()
    {
        await Interop.CopyToClipboard(GameCode!);
    }

    protected async void Restart()
    {
        flashboard!.ClearBoard();
        notificationMessage = string.Empty;
        await Interop.Broadcast("restart", "");
    }

    #endregion
}