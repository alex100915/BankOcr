namespace BankOcr.Services
{
    public class FileReader : IFileReader
    {
        public async Task<List<string>> ReadFileAsync(string path)
        {
            var content = await File.ReadAllTextAsync(path);

            return content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
        }
    }
}
