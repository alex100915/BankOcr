namespace BankOcr.Exceptions
{
    [Serializable]
    public class ParsingNumberException : Exception
    {
        public ParsingNumberException(string message) : base(message)
        {
        }
    }
}