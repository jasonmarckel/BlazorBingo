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

    // [JSMarshalAs<JSType.Any>] object component

    [JSImport("host", "PeerJs")]
    internal static partial Task Host([JSMarshalAs<JSType.String>] string id);

    [JSImport("connect", "PeerJs")]
    internal static partial Task Connect([JSMarshalAs<JSType.String>] string id, [JSMarshalAs<JSType.String>] string playerName);

    [JSImport("send", "PeerJs")]
    internal static partial Task Send([JSMarshalAs<JSType.String>] string id, [JSMarshalAs<JSType.String>] string message);

    [JSImport("broadcast", "PeerJs")]
    internal static partial Task Broadcast([JSMarshalAs<JSType.String>] string message);

    [JSExport]
    internal static void OnReceive(string json)
    {
        //DetectHands detectHands = (DetectHands)component;
        //detectHands.DetectionResult = JsonSerializer.Deserialize<DetectionResult>(json, DetectionResult.SerializeOptions);
        Console.WriteLine("OnReceive " + json); // detectHands.DetectionResult!.Hands.Count);
        //detectHands.StateHasChanged();
    }

}

