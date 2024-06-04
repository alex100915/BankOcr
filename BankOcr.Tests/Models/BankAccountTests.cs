using BankOcr.Models;
using BankOcr.Constants;

namespace BankOcr.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void AccountNumber_WithValidChecksum_ShouldReturnEmptyStatus()
        {
            // Arrange
            var account = new BankAccount();
            var validAccountNumber = "345882865"; // Example valid account number based on checksum logic

            // Act
            account.AccountNumber = validAccountNumber;

            // Assert
            Assert.That(account.AccountNumber, Is.EqualTo(validAccountNumber));
            Assert.That(account.Status, Is.EqualTo(string.Empty));
        }

        [Test]
        public void AccountNumber_WithInvalidChecksum_ShouldReturnChecksumInvalidStatus()
        {
            // Arrange
            var account = new BankAccount();
            var invalidAccountNumber = "123456289"; // Example invalid account number based on checksum logic

            // Act
            account.AccountNumber = invalidAccountNumber;

            // Assert
            Assert.That(account.AccountNumber, Is.EqualTo(invalidAccountNumber));
            Assert.That(account.Status, Is.EqualTo(BankAccountStatus.ChecksumInvalid));
        }

        [Test]
        public void AccountNumber_WithIllegibleCharacter_ShouldReturnIllegibleStatus()
        {
            // Arrange
            var account = new BankAccount();
            var illegibleAccountNumber = "12345?789"; // Example account number with illegible character

            // Act
            account.AccountNumber = illegibleAccountNumber;

            // Assert
            Assert.That(account.AccountNumber, Is.EqualTo(illegibleAccountNumber));
            Assert.That(account.Status, Is.EqualTo(BankAccountStatus.Illegible));
        }

        [Test]
        public void AccountNumber_ToString_ShouldReturnAccountNumber()
        {
            // Arrange
            var account = new BankAccount();
            var validAccountNumber = "123456789"; // Example of valid account number
            account.AccountNumber = validAccountNumber;
            var expectedOutput = $"{validAccountNumber}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void ToString_AmbiguousAccountNumber_ReturnsAccountNumberWithStatusAndAmbiguity()
        {
            // Arrange
            var bankAccount = new BankAccount
            {
                AccountNumber = "123456789",
                Ambiguity = @"[""123456780"", ""123456781""]"
            };

            // Act
            var result = bankAccount.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(@"123456789 AMB [""123456780"", ""123456781""]"));
        }
    }
}
