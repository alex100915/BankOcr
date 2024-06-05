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
        public void ParseFromOcr_UnrecognizableOcrNumber_ReportsIllegibleAccount()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = "    _  _     _  _  _  _  _ ",
                    Line2 = "  | _| _||_||_ |_   ||_||_|",
                    Line3 = "  ||_  _|  | _||_|  ||_||_ "  // Last digit is not a recognizable OCR number even after correction
                }
            };

            // Act & Assert
            Assert.That(_parser.ParseFromOcr(ocrBankAccounts).First().ToString(), Does.Contain("ILL"));
        }

        [Test]
        public void ParseFromOcr_InputForCorrection_ReturnsCorrectedAccountNumber()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = "    _  _     _  _  _  _  _ ",
                    Line2 = "  | _| _||_||_ |_   ||_||_|",
                    Line3 = "  ||_  _|  | _||_|  ||_|  |"  // Last digit is not a recognizable OCR number
                }
            };

            var bankAccounts = _parser.ParseFromOcr(ocrBankAccounts);

            // Assert
            Assert.That(bankAccounts.Count, Is.EqualTo(1));
            Assert.That(bankAccounts[0].AccountNumber, Is.EqualTo("123456789"));
        }

        [Test]
        public void ParseFromOcr_InputForCorrectionAndAmbiguous_ReturnsAmbiguousAccountNumbers()
        {
            // Arrange
            var ocrBankAccounts = new List<OcrBankAccount>
            {
                new OcrBankAccount
                {
                    Line1 = " _  _  _  _  _  _  _  _  _ ",
                    Line2 = "|_||_||_||_||_||_||_||_||_|",
                    Line3 = " _| _| _| _| _| _| _| _| _|"
                }
            };
            
            Assert.That(_parser.ParseFromOcr(ocrBankAccounts).First().ToString(), Is.EqualTo("999999999 AMB [\"899999999\",\"993999999\",\"999959999\"]"));
        }
    }
}
