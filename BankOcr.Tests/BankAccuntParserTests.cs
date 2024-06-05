using BankOcr.Exceptions;
using BankOcr.Models;

namespace BankOcr.Tests
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
    }
}
