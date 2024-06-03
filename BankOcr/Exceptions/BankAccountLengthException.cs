namespace BankOcr.Exceptions
{
    [Serializable]
    public class BankAccountLengthException : Exception
    {
        public BankAccountLengthException(string message) : base(message)
        {
        }
    }
}