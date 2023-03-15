using Microsoft.AspNetCore.Components;

namespace BlazorBingo.Client.Shared;

public partial class Flashboard
{
    [Parameter]
    public int BoardSize { get; set; }

    public bool IsFinishedCalling
    {
        get
        {
            return CalledNumbers.Count == BoardSize;
        }
    }

    public List<int> CalledNumbers { get; private set; } = null !;
    private int lastCalled;
    private int z;
    private const string BINGO = "BINGO";

    protected override void OnInitialized()
    {
        z = BoardSize / 5;
        CalledNumbers = new List<int>(capacity: BoardSize);
        lastCalled = 0;
    }

    private string GetCssClass(int n)
    {
        if (lastCalled == n)
        {
            return "lastCalled";
        }

        return CalledNumbers.Contains(n) ? "visible" : "invisible";
    }

    public void ClearBoard()
    {
        CalledNumbers.Clear();
        lastCalled = 0;
        this.StateHasChanged(); // force the UI to refresh
    }

    public void UpdateCalledNumbersCSV(string value)
    {
        CalledNumbers.Clear();
        CalledNumbers.AddRange(value.Split(",").Select(i => Int32.Parse(i)));
        lastCalled = CalledNumbers.LastOrDefault();
        this.StateHasChanged(); // force the UI to refresh
    }

    public string LastCalled
    {
        get
        {
            int col = ((lastCalled - 1) / z);
            return $"{BINGO[col]}-{lastCalled}";
        }
    }

    public string CalledNumbersCSV
    {
        get
        {
            return String.Join(",", CalledNumbers);
        }
    }

    public void Pick()
    {
        int x;
        do
        {
            x = ThreadSafeRandom.ThisThreadsRandom.Next(1, BoardSize + 1);
        }
        while (CalledNumbers.Contains(x));
        CalledNumbers.Add(x);
        lastCalled = x;
        StateHasChanged(); // refresh the UI
    }
}