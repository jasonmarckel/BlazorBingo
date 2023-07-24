using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/import-export-interop?view=aspnetcore-7.0#call-javascript-from-net

namespace BlazorBingo.Client;

[SupportedOSPlatform("browser")]
public partial class Interop
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(JsonTypeInfo))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(JsonSerializerContext))]
    public Interop()
	{
    }

    [JSImport("host", "interopModule")]
    internal static partial Task Host(
        [JSMarshalAs<JSType.Any>] object component,
        [JSMarshalAs<JSType.String>] string id);

    [JSImport("connect", "interopModule")]
    internal static partial Task Connect(
        [JSMarshalAs<JSType.Any>] object component, 
        [JSMarshalAs<JSType.String>] string remoteId, 
        [JSMarshalAs<JSType.String>] string playerName);

    [JSImport("broadcast", "interopModule")]
    internal static partial Task Broadcast(
        [JSMarshalAs<JSType.String>] string messageType,
        [JSMarshalAs<JSType.String>] string message);

    [JSImport("notifyHost", "interopModule")]
    internal static partial Task NotifyHost(
        [JSMarshalAs<JSType.String>] string messageType,
        [JSMarshalAs<JSType.String>] string message);

    [JSExport]
    internal static async void OnDataReceived(
        [JSMarshalAs<JSType.Any>] object component,
        [JSMarshalAs<JSType.String>] string messageType,
        [JSMarshalAs<JSType.String>] string data)
    {
        await ((IMessageHandler)component).HandleMessage(messageType, data);
    }

    [JSImport("shareUrl", "interopModule")]
    internal static partial Task ShareUrl(
        [JSMarshalAs<JSType.String>] string title,
        [JSMarshalAs<JSType.String>] string text,
        [JSMarshalAs<JSType.String>] string? url);

    // the Clipboard feature is available only in secure contexts (HTTPS)
    // https://developer.mozilla.org/en-US/docs/Web/API/Clipboard

    [JSImport("globalThis.navigator.clipboard.writeText")]
    internal static partial Task CopyToClipboard([JSMarshalAs<JSType.String>] string text);

    [JSImport("showModal", "interopModule")]
    internal static partial Task ShowModal([JSMarshalAs<JSType.String>] string id);

    [JSImport("getLanguage", "interopModule")]
    internal static partial Task<string> GetLanguage();

    [JSImport("getUserAgent", "interopModule")]
    internal static partial Task<string> GetUserAgent();

    [JSImport("isIOS", "interopModule")]
    internal static partial Task<bool> IsIOS();

    [JSImport("isInStandaloneMode", "interopModule")]
    internal static partial Task<bool> IsInStandaloneMode();

    [JSImport("isInstalled", "interopModule")]
    internal static partial Task<bool> IsInstalled();

    [JSImport("installPWA", "interopModule")]
    internal static partial Task InstallPWA();

    [JSImport("initVoices", "interopModule")]
    internal static partial Task InitVoices();

    [JSImport("getVoices", "interopModule")]
    internal static partial Task<string> GetVoices();

    [JSImport("speak", "interopModule")]
    internal static partial Task Speak(
        [JSMarshalAs<JSType.String>] string inputText, 
        [JSMarshalAs<JSType.String>] string voiceName,
        [JSMarshalAs<JSType.String>] string language);

    [JSImport("primeSpeechSynthesis", "interopModule")]
    internal static partial Task PrimeSpeechSynthesis();

    [JSImport("requestWakeLock", "interopModule")]
    internal static partial Task RequestWakeLock();

    [JSImport("releaseWakeLock", "interopModule")]
    internal static partial Task ReleaseWakeLock();
}
