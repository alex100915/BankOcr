using BankOcr.Exceptions;
using BankOcr.Models;

namespace BankOcr.Tests.Services
{
    [TestFixture]
    public class BankAccountParserTests
    {
        private readonly BankAccountParser _parser = new BankAccountParser();

        [Test]
        public void ParseFromOcr_ValidInput_ReturnsBankAccounts()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = "    _  _     _  _  _  _  _ ",
                    Line2 = "  | _| _||_||_ |_   ||_||_|",
                    Line3 = "  ||_  _|  | _||_|  ||_| _|"
                }
            };

            // Act
            var bankAccounts = _parser.ParseFromOcr(ocrBankAccounts);

            // Assert
            Assert.That(bankAccounts.Count, Is.EqualTo(1));
            Assert.That(bankAccounts[0].AccountNumber, Is.EqualTo("123456789"));
        }

        [Test]
        public void ParseFromOcr_InvalidLineLength_ThrowsBankAccountLengthException()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = "    _  _     _  _  _  _  _",  // 26 characters instead of 27
                    Line2 = "  | _| _||_||_ |_   ||_||_|",
                    Line3 = "  ||_  _|  | _||_|  ||_| _|"
                }
            };

            // Act & Assert
            Assert.Throws<BankAccountLengthException>(() => _parser.ParseFromOcr(ocrBankAccounts));
        }

        [Test]
        public void ParseFromOcr_UnrecognizableOcrNumber_ThrowsParsingNumberException()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = "    _  _     _  _  _  _  _ ",
                    Line2 = "  | _| _||_||_ |_   ||_||_|",
                    Line3 = "  ||_  _|  | _||_|  ||_||_ "  // Last digit is not a recognizable OCR number
                }
            };

            // Act & Assert
            Assert.Throws<ParsingNumberException>(() => _parser.ParseFromOcr(ocrBankAccounts));
        }

        [Test]
        public void GetOcrNumbers_ValidInput_ReturnsOcrNumbers()
        {
            // Arrange
            var ocrBankAccount = new OcrBankAccount
            {
                Line1 = "    _  _     _  _  _  _  _ ",
                Line2 = "  | _| _||_||_ |_   ||_||_|",
                Line3 = "  ||_  _|  | _||_|  ||_| _|"
            };

            // Act
            var ocrNumbers = _parser.GetOcrNumbers(ocrBankAccount);

            // Assert
            Assert.That(ocrNumbers.Count, Is.EqualTo(9));
            Assert.That(ocrNumbers[0], Is.EqualTo("     |  |"));
            Assert.That(ocrNumbers[1], Is.EqualTo(" _  _||_ "));
            Assert.That(ocrNumbers[8], Is.EqualTo(" _ |_| _|"));
        }

        [Test]
        public void ValidateOcrBankAccount_InvalidLineLength_ThrowsBankAccountLengthException()
        {
            // Arrange
            var ocrBankAccount = new OcrBankAccount
            {
                Line1 = "    _  _     _  _  _  _  _",  // 26 characters instead of 27
                Line2 = "  | _| _||_||_ |_   ||_||_|",
                Line3 = "  ||_  _|  | _||_|  ||_| _|"
            };

            // Act & Assert
            var ex = Assert.Throws<BankAccountLengthException>(() => _parser.GetOcrNumbers(ocrBankAccount));
            Assert.That(ex.Message, Is.EqualTo("Each line in input file should have exactly 27 characters"));
        }
    }
}
