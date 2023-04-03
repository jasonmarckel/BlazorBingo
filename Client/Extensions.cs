namespace BlazorBingo.Client;

public static class Extensions
{
    // Fisher-Yates shuffle
    // https://stackoverflow.com/questions/273313/randomize-a-listt

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}