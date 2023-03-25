using Microsoft.AspNetCore.Components;
using System.Runtime.Versioning;
using System.Text.Json;

namespace BlazorBingo.Client.Pages;

[SupportedOSPlatform("browser")]
public partial class SettingsScreen
{
    [Parameter]
    public bool isGameStarted { get; set; }

    protected bool isLoaded;
    protected bool showVoiceSelection;
    protected IEnumerable<Voice>? voices;
    protected string? userAgent;
    protected string? platform;
    protected string? language;

    private int _gamePatternIndex;
    protected int gamePatternIndex
    {
        get { return _gamePatternIndex; }
        set
        {
            if (value < 0) { _gamePatternIndex = GamePatterns.GetCount() - 1; }
            else if (value >= GamePatterns.GetCount()) { _gamePatternIndex = 0; }
            else _gamePatternIndex = value;
            settings.SelectedPattern = GamePatterns.GetPatternName(_gamePatternIndex);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Interop.InitVoices();
        string voicesJson = await Interop.GetVoices();
        voices = JsonSerializer.Deserialize<IEnumerable<Voice>>(voicesJson)?.Where(x => !x.name.StartsWith("Android"));
        userAgent = await Interop.GetUserAgent();
        platform = await Interop.GetPlatform();
        language = await Interop.GetLanguage();
        isLoaded = true;
        showVoiceSelection = !userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase) || userAgent.Contains("Surface Duo", StringComparison.OrdinalIgnoreCase);
        gamePatternIndex = GamePatterns.GetPatternNames().ToList().IndexOf(settings.SelectedPattern);
    }
    protected void ToggleMute()
    {
        settings.IsMuted = !settings.IsMuted;
    }
    protected void SelectDauber(string val)
    {
        settings.Dauber = val;
    }

    public static readonly string[] CardThemes = new string[]
    {
        "Black",
        "Blue",
        "Green",
        "Orange",
        "Red",
        "Yellow"
    };

    // https://symbl.cc

    public static Dictionary<string, string> Daubers = new()
    {
        { "Random", "&#10067;" },
        { "Airplane", "&#9992;" },
        { "Banana", "&#127820;" },
        { "Basketball", "&#127936;" },
        { "Beer Mug", "&#127866;" },
        { "Cactus", "&#127797;" },
        { "Cat", "&#128008;" },
        { "Cat Face with Heart Eyes", "&#128571;" },
        { "Cherries", "&#127826;" },
        { "Chess Pawn", "&#9823;" },
        { "Christmas Tree", "&#127876;" },
        { "Cookie", "&#127850;" },
        { "Curling", "&#129356;" },
        { "Dog Face", "&#128054;" },
        { "Doughnut", "&#127849;" },
        { "Dragon", "&#128009;" },
        { "Duck", "&#129414;" },
        { "Elephant", "&#128024;" },
        { "Fire", "&#128293;" },
        { "Flying Saucer", "&#128760;" },
        { "Football", "&#127944;" },
        { "Four Leaf Clover", "&#127808;" },
        { "Fox", "&#129418;" },
        { "Golf Course", "&#9971;" },
        { "Golfer", "&#127948;" },
        { "Heart", "&#10084;" },
        { "Hot Dog", "&#127789;" },
        { "Ice Cream", "&#127846;" },
        { "Kangaroo", "&#129432;" },
        { "Kiss", "&#128139;" },
        { "Money Bag", "&#128176;" },
        { "Musical Note", "&#127925;" },
        { "Paw Prints", "&#128062;" },
        { "Penguin", "&#128039;" },
        { "Pineapple", "&#127821;" },
        { "Ping Pong", "&#127955;" },
        { "Present", "&#127873;" },
        { "Pumpkin", "&#127875;" },
        { "Puzzle Piece", "&#129513;" },
        { "Scooter", "&#128757;" },
        { "Shark", "&#129416;" },
        { "Skull", "&#128128;" },
        { "Skull & Crossbones", "&#9760;" },
        { "Smiley", "&#128515;" },
        { "Snowman", "&#9924;" },
        { "Soccer", "&#9917;" },
        { "Strawberry", "&#127827;" },
        { "T-Rex", "&#129430;" },
        { "Tennis", "&#127934;" },
        { "Tractor", "&#128668;" },
        { "Trident", "&#128305;" },
        { "Video Game Controller", "&#127918;" },
        { "Water Wave", "&#127754;" }
    };
    public record Voice
    {
        public required string name { get; set; }

        public required string lang { get; set; }

        public bool isDefault { get; set; }

        public bool localService { get; set; }
    }
}