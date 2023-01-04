using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace BlazorBingo;

[SupportedOSPlatform("browser")]
public partial class PeerInterop
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(JsonTypeInfo))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(JsonSerializerContext))]
    public PeerInterop()
	{
    }

    [JSImport("host", "PeerJs")]
    internal static partial Task Host([JSMarshalAs<JSType.String>] string id);

    [JSImport("connect", "PeerJs")]
    internal static partial Task Connect([JSMarshalAs<JSType.String>] string remoteId, [JSMarshalAs<JSType.String>] string playerName);

    [JSImport("broadcast", "PeerJs")]
    internal static partial Task Broadcast([JSMarshalAs<JSType.String>] string message);

    [JSImport("notifyHost", "PeerJs")]
    internal static partial Task NotifyHost([JSMarshalAs<JSType.String>] string message);

    [JSExport]
    internal static void OnConnected(string playerName)
    {
        Console.WriteLine(playerName + " connected.");
    }

    [JSExport]
    internal static void OnDisconnected(string playerName)
    {
        Console.WriteLine(playerName + " disconnected.");
    }

    [JSExport]
    internal static void OnDataReceived(string data)
    {
        // [JSMarshalAs<JSType.Any>] object component
        //DetectHands detectHands = (DetectHands)component;
        //detectHands.DetectionResult = JsonSerializer.Deserialize<DetectionResult>(json, DetectionResult.SerializeOptions);
        Console.WriteLine("OnDataReceived " + data);
        //detectHands.StateHasChanged();
    }

}

