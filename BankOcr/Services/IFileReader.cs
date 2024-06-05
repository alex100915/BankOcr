namespace BankOcr.Services
{
    public interface IFileReader
    {
        Task<List<string>> ReadFileAsync(string path);
    }
}