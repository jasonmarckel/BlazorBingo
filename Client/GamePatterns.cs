namespace BlazorBingo.Client;

public class GamePatterns
{
    // straight line patterns
    //
    // XXXXX ..... ..... ..... ..... X.... .X... ..X.. ...X. ....X X.... ....X
    // ..... XXXXX ..... ..... ..... X.... .X... ..X.. ...X. ....X .X... ...X.
    // ..... ..... XXXXX ..... ..... X.... .X... ..X.. ...X. ....X ..X.. ..X..
    // ..... ..... ..... XXXXX ..... X.... .X... ..X.. ...X. ....X ...X. .X...
    // ..... ..... ..... ..... XXXXX X.... .X... ..X.. ...X. ....X ....X X....

    public const string Default = "Straight Line";

    // 0U == placeholder for set of patterns

    private static readonly Dictionary<string, uint> PatternsDict = new()
    {
        { Default, 0U },
        { "Airplane", 0b00000000001010010111111001000010 },
        { "Alien", 0b00000001111110101111110111011011 },
        { "Arrow", 0b00000001111011000101001001000001 },
        { "Arrowhead", 0b00000000011100011000010000000000 },
        { "Forward Slash", 0b00000000000100010001000100010000 },
        { "Backslash", 0b00000001000001000001000001000001 },
        { "Barbell", 0b00000000000010001111111000100000 },
        { "Blackout", 0b00000001111111111111111111111111 },
        { "Block Party", 0b00000001110111100111000000010001 },
        { "Block of 9", 0b00000000000000000111001110011100 },
        { "Bow Tie", 0b00000001000111011111111101110001 },
        { "Box Large", 0b00000001111110001100011000111111 }, // Row1 | Col5 | Row5 | Col1
        { "Box Small", 0b00000000000001110010100111000000 },
        { "Checkerboard", 0b00000001010101010101010101010101 },
        { "Checkmark", 0b00000000000100010101001100010000 },
        { "Christmas Tree", 0b00000000010001110111110010000100 },
        { "Column B", 0b00000001000010000100001000010000 },
        { "Column I", 0b00000000100001000010000100001000 },
        { "Column N", 0b00000000010000100001000010000100 },
        { "Column G", 0b00000000001000010000100001000010 },
        { "Column O", 0b00000000000100001000010000100001 },
        { "B N & O", 0b00000001010110101101011010110101 },   // Col1 | Col3 | Col5
        { "Corner NE", 0b00000001111100001000010000100001 },
        { "Corner NW", 0b00000001111110000100001000010000 },
        { "Corner SE", 0b00000000000100001000010000111111 },
        { "Corner SW", 0b00000001000010000100001000011111 },
        { "Crazy T", 0b00000000000100001111110000100001 },
        { "Cross", 0b00000000010011111001000010000100 },
        { "Diamond Full", 0b00000000010001110111110111000100 },
        { "Diamond Large", 0b00000000010001010100010101000100 },
        { "Diamond Small", 0b00000000000000100010100010000000 },
        { "Double Chevron", 0b00000000010001010101010101010001 },
        { "Downhill", 0b00000001000011000111001111011111 },
        { "Dumbell", 0b00000000000001010111110101000000 },
        { "Field Goal", 0b00000001010110001111110010000100 },
        { "Four Angles", 0b00000001101110001000001000111011 },
        { "Four Blocker", 0b00000001101111011000001101111011 },
        { "Four Corners", 0b00000001000100000000000000010001 },
        { "Hash", 0b00000000101011111010101111101010 }, // Row2 | Row4 | Col2 | Col4
        { "Heart", 0b00000001101110101100010101000100 },
        { "House", 0b00000000010001110111111101111011 },
        { "Layer Cake", 0b00000001111100000111110000011111 }, // Row1 | Row3 | Row5
        { "Letter A", 0b00000000010001010111111000110001 },
        { "Letter B", 0b00000001111010001111101000111110 },
        { "Letter C", 0b00000000111010000100001000001110 },
        { "Letter D", 0b00000001111010001100011000111110 },
        { "Letter E", 0b00000001111110000111101000011111 },
        { "Letter F", 0b00000001111110000111101000010000 },
        { "Letter G", 0b00000001111110000101111000111110 },
        { "Letter H", 0b00000001000110001111111000110001 }, // Col1 | Row3 | Col5
        { "Letter I", 0b00000001111100100001000010011111 }, // Row1 | Col3 | Row5
        { "Letter J", 0b00000000111100010100101001001100 },
        { "Letter K", 0b00000000100101010011000101001001 },
        { "Letter L", 0b00000000100001000010000100001111 },
        { "Letter M", 0b00000001000111011101011000110001 },
        { "Letter N", 0b00000001000111001101011001110001 }, // Col1 | Backslash | Col5
        { "Letter O", 0b00000000111010001100011000101110 },
        { "Letter P", 0b00000001111010010111101000010000 },
        { "Letter Q", 0b00000000110010010100101001001101 },
        { "Letter R", 0b00000001110010010111001010010010 },
        { "Letter S", 0b00000001111110000111110000111111 },
        { "Letter T", 0b00000001111100100001000010000100 },
        { "Letter U", 0b00000001000110001100011000111111 },
        { "Letter V", 0b00000001000110001100010101000100 },
        { "Letter W", 0b00000001000110001101011101110001 },
        { "Letter X", 0b00000001000101010001000101010001 }, // ForwardSlash | Backslash
        { "Letter Y", 0b00000001000110001011100010000100 },
        { "Letter Z", 0b00000001111100010001000100011111 }, // Row1 | ForwardSlash | Row5
        { "Little Jackpot", 0b00000001000101110010100111010001 },
        { "Missing Link", 0b00000000111110001100011000111111 },
        { "Plus Sign", 0b00000000010000100111110010000100 },
        { "Postage Stamp", 0b00000000001100011000000000000000 },
        { "Pyramid", 0b00000000000000000001000111011111 },
        { "Railroad", 0b00000000101001010010100101001010 },
        { "Rollercoaster", 0b00000000100110101101011010110010 },
        { "Roving L", 0U },
        { "Row 1", 0b00000001111100000000000000000000 },
        { "Row 2", 0b00000000000011111000000000000000 },
        { "Row 3", 0b00000000000000000111110000000000 },
        { "Row 4", 0b00000000000000000000001111100000 },
        { "Row 5", 0b00000000000000000000000000011111 },
        { "Six Pack", 0b00000000000011000110001100000000 },
        { "Tic-Tac-Toe", 0b00000001010100000101010000010101 },
        { "Top & Bottom", 0b00000001111100000000000000011111 },
        { "Waterfall", 0b00000001100011100001000011100011 },
        { "Waves", 0b00000000101010101000000101010101 },
        { "Worm", 0b00000000111001010010100101011011 },
    };

    public static IEnumerable<string> GetPatternNames()
    {
        return PatternsDict.Keys;
    }
    public static int GetCount()
    {
        return PatternsDict.Count;
    }
    public static uint GetPattern(string patternName)
    {
        return PatternsDict[patternName];
    }
    public static string GetPatternName(int patternIndex)
    {
        return PatternsDict.ElementAt(patternIndex).Key;
    }
    public static IEnumerable<uint> GetPatternSet(string patternName)
    {
        switch (patternName)
        {
            case Default:
                yield return PatternsDict["Row 1"];
                yield return PatternsDict["Row 2"];
                yield return PatternsDict["Row 3"];
                yield return PatternsDict["Row 4"];
                yield return PatternsDict["Row 5"];
                yield return PatternsDict["Column B"];
                yield return PatternsDict["Column I"];
                yield return PatternsDict["Column N"];
                yield return PatternsDict["Column G"];
                yield return PatternsDict["Column O"];
                yield return PatternsDict["Forward Slash"];
                yield return PatternsDict["Backslash"];
                break;
            case "Roving L":
                yield return PatternsDict["Corner NW"];
                yield return PatternsDict["Corner NE"];
                yield return PatternsDict["Corner SW"];
                yield return PatternsDict["Corner SE"];
                break;
            default:
                yield return PatternsDict[patternName];
                break;
        }        
    }
}
