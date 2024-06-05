using BankOcr.Services;

namespace BankOcr.Tests.Services
{
    [TestFixture]
    public class FileReaderTests
    {
        private FileReader _fileReader;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new FileReader();
        }

        [Test]
        public async Task ReadFileAsync_FileExists_ReturnsFileLines()
        {
            // Arrange
            string path = "validFilePath.txt";
            string fileContent = "line1\nline2\nline3";
            File.WriteAllText(path, fileContent);

            // Act
            var result = await _fileReader.ReadFileAsync(path);

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo("line1"));
            Assert.That(result[1], Is.EqualTo("line2"));
            Assert.That(result[2], Is.EqualTo("line3"));

            // Cleanup
            File.Delete(path);
        }

        [Test]
        public void ReadFileAsync_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            string path = "invalidFilePath.txt";

            // Act & Assert
            var ex = Assert.ThrowsAsync<FileNotFoundException>(async () => await _fileReader.ReadFileAsync(path));
            Assert.That(ex.Message, Does.Contain("Could not find file"));
        }

        [Test]
        public async Task ReadFileAsync_FileWithEmptyLines_ReturnsListIncludingEmptyLines()
        {
            // Arrange
            string path = "fileWithEmptyLines.txt";
            string fileContent = "line1\n\nline3\n";
            await File.WriteAllTextAsync(path, fileContent);

            // Act
            var result = await _fileReader.ReadFileAsync(path);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0], Is.EqualTo("line1"));
            Assert.That(result[1], Is.EqualTo(string.Empty));
            Assert.That(result[2], Is.EqualTo("line3"));
            Assert.That(result[3], Is.EqualTo(string.Empty));

            // Cleanup
            File.Delete(path);
        }
    }
}
