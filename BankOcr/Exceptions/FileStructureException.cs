namespace BankOcr.Exceptions
{
    [Serializable]
    public class FileStructureException : Exception
    {
        public FileStructureException(string message) : base(message)
        {
        }
    }
}