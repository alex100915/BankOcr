namespace BankOcr.Constants
{
    public class OcrNumbers
    {
        const string Zero =
            " _ " +
            "| |" +
            "|_|";

        const string One =
            "   " +
            "  |" +
            "  |";

        const string Two =
            " _ " +
            " _|" +
            "|_ ";

        const string Three =
            " _ " +
            " _|" +
            " _|";

        const string Four =
            "   " +
            "|_|" +
            "  |";

        const string Five =
            " _ " +
            "|_ " +
            " _|";

        const string Six =
            " _ " +
            "|_ " +
            "|_|";

        const string Seven =
            " _ " +
            "  |" +
            "  |";

        const string Eight =
            " _ " +
            "|_|" +
            "|_|";

        const string Nine =
            " _ " +
            "|_|" +
            " _|";

        public static Dictionary<string, int> OcrNumbersDictionary = new Dictionary<string, int>
        {
            { Zero, 0 },
            { One, 1 },
            { Two, 2 },
            { Three, 3 },
            { Four, 4},
            { Five, 5 },
            { Six, 6},
            { Seven, 7 },
            { Eight, 8 },
            { Nine, 9 }
        };
    }
}
