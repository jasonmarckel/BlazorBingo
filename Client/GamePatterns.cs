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

    public const string Default = "-StraightLine-";

    // 0U == placeholder for set of patterns

    private static SortedDictionary<string, uint> PatternsDict = new()
    {
        { Default, 0U },
        { "RovingL", 0U },
        { "Row1", 0b00000001111100000000000000000000 },
        { "Row2", 0b00000000000011111000000000000000 },
        { "Row3", 0b00000000000000000111110000000000 },
        { "Row4", 0b00000000000000000000001111100000 },
        { "Row5", 0b00000000000000000000000000011111 },
        { "Column1", 0b00000001000010000100001000010000 },
        { "Column2", 0b00000000100001000010000100001000 },
        { "Column3", 0b00000000010000100001000010000100 },
        { "Column4", 0b00000000001000010000100001000010 },
        { "Column5", 0b00000000000100001000010000100001 },
        { "Backslash", 0b00000001000001000001000001000001 },
        { "ForwardSlash", 0b00000000000100010001000100010000 },
        { "LayerCake", 0b00000001111100000111110000011111 }, // Row1 | Row3 | Row5
        { "BN&O", 0b00000001010110101101011010110101 },   // Col1 | Col3 | Col5
        { "Railroad", 0b00000000101001010010100101001010 },
        { "BowTie", 0b00000001000111011111111101110001 },
        { "ChristmasTree", 0b00000000010001110111110010000100 },
        { "Checkmark", 0b00000000000100010101001100010000 },
        { "MissingLink", 0b00000000111110001100011000111111 },
        { "Arrow", 0b00000001111011000101001001000001 },
        { "Checkerboard", 0b00000001010101010101010101010101 },
        { "PlusSign", 0b00000000010000100111110010000100 },
        { "Heart", 0b00000001101110101100010101000100 },
        { "Blackout", 0b00000001111111111111111111111111 },
        { "PostageStamp", 0b00000000001100011000000000000000 },
        { "FourCorners", 0b00000001000100000000000000010001 },
        { "FourBlocker", 0b00000001101111011000001101111011 },
        { "FourAngles", 0b00000001101110001000001000111011 },
        { "Cross", 0b00000000010011111001000010000100 },
        { "BoxLarge", 0b00000001111110001100011000111111 }, // Row1 | Col5 | Row5 | Col1
        { "BoxSmall", 0b00000000000001110010100111000000 },
        { "DiamonFull", 0b00000000010001110111110111000100 },
        { "DiamondLarge", 0b00000000010001010100010101000100 },
        { "DiamondSmall", 0b00000000000000100010100010000000 },
        { "Top&Bottom", 0b00000001111100000000000000011111 },
        { "Hash", 0b00000000101011111010101111101010 }, // Row2 | Row4 | Col2 | Col4
        { "Alien1", 0b00000001111110101111110111011011 },
        { "Alien2", 0b00000000010001010101010101010001 },
        { "Waves", 0b00000000101010101000000101010101 },
        { "Worm", 0b00000000111001010010100101011011 },
        { "Rollercoaster", 0b00000000100110101101011010110010 },
        { "House", 0b00000000010001110111111101111011 },
        { "SixPack", 0b00000000000011000110001100000000 },
        { "BlockOf9", 0b00000000000000000111001110011100 },
        { "LittleJackpot", 0b00000001000101110010100111010001 },
        { "BlockParty", 0b00000001110111100111000000010001 },
        { "CrazyT", 0b00000000000100001111110000100001 },
        { "Waterfall", 0b00000001100011100001000011100011 },
        { "Dumbell", 0b00000000000001010111110101000000 },
        { "Barbell", 0b00000000000010001111111000100000 },
        { "Arrowhead", 0b00000000011100011000010000000000 },
        { "Airplane", 0b00000000001010010111111001000010 },
        { "FieldGoal", 0b00000001010110001111110010000100 },
        { "Downhill", 0b00000001000011000111001111011111 },
        { "TicTacToe", 0b00000001010100000101010000010101 },
        { "Pyramid", 0b00000000000000000001000111011111 },
        { "LetterA", 0b00000000010001010111111000110001 },
        { "LetterB", 0b00000001111010001111101000111110 },
        { "LetterC", 0b00000000111010000100001000001110 },
        { "LetterD", 0b00000001111010001100011000111110 },
        { "LetterE", 0b00000001111110000111101000011111 },
        { "LetterF", 0b00000001111110000111101000010000 },
        { "LetterG", 0b00000001111110000101111000111110 },
        { "LetterH", 0b00000001000110001111111000110001 }, // Col1 | Row3 | Col5
        { "LetterI", 0b00000001111100100001000010011111 }, // Row1 | Col3 | Row5
        { "LetterJ", 0b00000000111100010100101001001100 },
        { "LetterK", 0b00000000100101010011000101001001 },
        { "LetterL", 0b00000000100001000010000100001111 },
        { "LetterM", 0b00000001000111011101011000110001 },
        { "LetterN", 0b00000001000111001101011001110001 }, // Col1 | Backslash | Col5
        { "LetterO", 0b00000000111010001100011000101110 },
        { "LetterP", 0b00000001111010010111101000010000 },
        { "LetterQ", 0b00000000110010010100101001001101 },
        { "LetterR", 0b00000001110010010111001010010010 },
        { "LetterS", 0b00000001111110000111110000111111 },
        { "LetterT", 0b00000001111100100001000010000100 },
        { "LetterU", 0b00000001000110001100011000111111 },
        { "LetterV", 0b00000001000110001100010101000100 },
        { "LetterW", 0b00000001000110001101011101110001 },
        { "LetterX", 0b00000001000101010001000101010001 }, // ForwardSlash | Backslash
        { "LetterY", 0b00000001000110001011100010000100 },
        { "LetterZ", 0b00000001111100010001000100011111 }, // Row1 | ForwardSlash | Row5
        { "Northwest", 0b00000001111110000100001000010000 },
        { "Northeast", 0b00000001111100001000010000100001 },
        { "Southwest", 0b00000001000010000100001000011111 },
        { "Southeast", 0b00000000000100001000010000111111 }
    };

    public static IEnumerable<string> GetPatternNames()
    {
        return PatternsDict.Keys;
    }

    public static uint GetPattern(string patternName)
    {
        return PatternsDict[patternName];
    }

    public static IEnumerable<uint> GetPatternSet(string patternName)
    {
        var returnValues = Enumerable.Empty<uint>();
        switch (patternName)
        {
            case Default:
                yield return PatternsDict["Row1"];
                yield return PatternsDict["Row2"];
                yield return PatternsDict["Row3"];
                yield return PatternsDict["Row4"];
                yield return PatternsDict["Row5"];
                yield return PatternsDict["Column1"];
                yield return PatternsDict["Column2"];
                yield return PatternsDict["Column3"];
                yield return PatternsDict["Column4"];
                yield return PatternsDict["Column5"];
                yield return PatternsDict["ForwardSlash"];
                yield return PatternsDict["Backslash"];
                break;
            case "RovingL":
                yield return PatternsDict["Northwest"];
                yield return PatternsDict["Northeast"];
                yield return PatternsDict["Southwest"];
                yield return PatternsDict["Southeast"];
                break;
            default:
                yield return PatternsDict[patternName];
                break;
        }        
    }
}
