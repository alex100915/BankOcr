namespace BankOcr.Exceptions
{
    [Serializable]
    public class OcrBankAccountLengthException : Exception
    {
        public OcrBankAccountLengthException(string message) : base(message)
        {
        }
    }
}