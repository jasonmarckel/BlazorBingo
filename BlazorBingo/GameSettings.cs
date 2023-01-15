namespace BlazorBingo;

public class GameSettings
{
    public string PlayerName { get; set; } = string.Empty;
    public string Dauber { get; set; } = "random";
    public string CallerVoice { get; set; } = string.Empty;
    public string CardTheme { get; set; } = "blue";
}
