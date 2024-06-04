namespace BankOcr.Services
{
    public interface IFileReader
    {
        List<string> ReadFile(string path);
    }
}