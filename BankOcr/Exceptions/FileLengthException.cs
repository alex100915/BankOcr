namespace BankOcr.Exceptions
{
    [Serializable]
    public class FileLengthException : Exception
    {
        public FileLengthException(string message) : base(message)
        {
        }
    }
}