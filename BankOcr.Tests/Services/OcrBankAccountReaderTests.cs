using BankOcr.Exceptions;
using BankOcr.Services;

namespace BankOcr.Tests.Services
{
    [TestFixture]
    public class OcrBankAccountReaderTests
    {
        private readonly OcrBankAccountReader _reader = new OcrBankAccountReader();

        [Test]
        public void Read_ValidInput_ReturnsBankAccounts()
        {
            // Arrange
            var fileLines = new List<string>
            {
                "    _  _     _  _  _  _  _ ",
                "  | _| _||_||_ |_   ||_||_|",
                "  ||_  _|  | _||_|  ||_| _|",
                "", // Empty line after first account
                " _  _  _  _  _  _  _  _  _ ",
                "| || || || || || || || || |",
                "|_||_||_||_||_||_||_||_||_|",
                ""  // Empty line after second account
            };

            // Act
            var bankAccounts = _reader.Read(fileLines);

            // Assert
            Assert.That(bankAccounts.Count, Is.EqualTo(2));
            Assert.That(bankAccounts[0].Line1, Is.EqualTo("    _  _     _  _  _  _  _ "));
            Assert.That(bankAccounts[0].Line2, Is.EqualTo("  | _| _||_||_ |_   ||_||_|"));
            Assert.That(bankAccounts[0].Line3, Is.EqualTo("  ||_  _|  | _||_|  ||_| _|"));
            Assert.That(bankAccounts[1].Line1, Is.EqualTo(" _  _  _  _  _  _  _  _  _ "));
            Assert.That(bankAccounts[1].Line2, Is.EqualTo("| || || || || || || || || |"));
            Assert.That(bankAccounts[1].Line3, Is.EqualTo("|_||_||_||_||_||_||_||_||_|"));
        }

        [Test]
        public void Read_InvalidFileLength_ThrowsFileLengthException()
        {
            // Arrange
            var fileLines = new List<string>
            {
                "    _  _     _  _  _  _  _ ",
                "  | _| _||_||_ |_   ||_||_|",
                "  ||_  _|  | _||_|  ||_| _|" // Missing empty line
            };

            // Act & Assert
            var ex = Assert.Throws<FileLengthException>(() => _reader.Read(fileLines));
            Assert.That(ex.Message, Is.EqualTo("Each bank account record should have exactly 3 lines followed by 1 empty line"));
        }

        [Test]
        public void Read_MissingEmptyLine_ThrowsFileStructureException()
        {
            // Arrange
            var fileLines = new List<string>
            {
                "    _  _     _  _  _  _  _ ",
                "  | _| _||_||_ |_   ||_||_|",
                "  ||_  _|  | _||_|  ||_| _|",
                "bonjour",// not empty line
                " _  _  _  _  _  _  _  _  _ ",
                "| || || || || || || || || |",
                "|_||_||_||_||_||_||_||_||_|",
                ""  // Empty line after second account
            };

            // Act & Assert
            var ex = Assert.Throws<FileStructureException>(() => _reader.Read(fileLines));
            Assert.That(ex.Message, Is.EqualTo("Each bank account should be followed by an empty line"));
        }


        [Test]
        public void Read_InvalidLineLength_ThrowsBankAccountLengthException()
        {
            // Arrange
            var fileLines = new List<string>
            {
                "    _  _     _  _  _  _  _", // 26 lines
                "  | _| _||_||_ |_   ||_||_|",
                "  ||_  _|  | _||_|  ||_| _|",
                "",
                " _  _  _  _  _  _  _  _  _ ",
                "| || || || || || || || || |",
                "|_||_||_||_||_||_||_||_||_|",
                ""  // Empty line after second account
            };

            // Act & Assert
            Assert.Throws<OcrBankAccountLengthException>(() => _reader.Read(fileLines));
        }
    }
}
