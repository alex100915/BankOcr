namespace BankOcr.Services
{
    public class FileReader
    {
        public List<string> ReadFile(string path)
        {
            var content = File.ReadAllText(path);

            return content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
        }
    }
}
