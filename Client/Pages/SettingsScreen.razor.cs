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
    }
    protected void ToggleMute()
    {
        settings.IsMuted = !settings.IsMuted;
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
    public static readonly string[] Daubers = new string[]
    {
        "Random",
        "Banana",
        "Basketball",
        "Beer Mug",
        "Cat",
        "Cat Face with Heart Eyes",
        "Cherries",
        "Christmas Tree",
        "Cookie",
        "Curling",
        "Dog Face",
        "Doughnut",
        "Dragon",
        "Elephant",
        "Fire",
        "Football",
        "Fox",
        "Golfer",
        "Heart",
        "Hot Dog",
        "Ice Cream",
        "Kangaroo",
        "Kiss",
        "Money Bag",
        "Musical Note",
        "Paw Prints",
        "Penguin",
        "Ping Pong",
        "Present",
        "Scooter",
        "Shark",
        "Smiley",
        "Snowman",
        "Soccer",
        "Strawberry",
        "T-Rex",
        "Tennis",
        "Tractor",
        "Trident",
        "Video Game Controller",
        "Water Wave"
    };
    public record Voice
    {
        public required string name { get; set; }

        public required string lang { get; set; }

        public bool isDefault { get; set; }

        public bool localService { get; set; }
    }
}