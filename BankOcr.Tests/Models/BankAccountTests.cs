using BankOcr.Models;
using BankOcr.Constants;

namespace BankOcr.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void BankAccount_ToString_ShouldReturnAccountNumber()
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
        public void BankAccount_ToString_ShouldReturnAccountNumberWithIllegibleStatus()
        {
            // Arrange
            var account = new BankAccount();
            var validAccountNumber = "123456789"; // Example of valid account number
            account.AccountNumber = validAccountNumber;
            account.Status = BankAccountStatus.Illegible;

            var expectedOutput = $"{validAccountNumber} {account.Status}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BankAccount_ToString_ShouldReturnAccountNumberWithAmbigiousStatusAndNumbers()
        {
            // Arrange
            var account = new BankAccount();
            var validAccountNumber = "123456789";

            account.AccountNumber = validAccountNumber;
            account.Status = BankAccountStatus.Ambiguous;
            account.Ambiguity = @"[""123456780"", ""123456781""]";

            var expectedOutput = $"{validAccountNumber} {account.Status} {account.Ambiguity}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnFalseWhenNumberChecksumInvalid()
        {
            // Arrange
            var account = new BankAccount();
            var invalidAccountNumber = "123433739";

            account.AccountNumber = invalidAccountNumber;

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(false));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnTrueWhenNumberChecksumValid()
        {
            // Arrange
            var account = new BankAccount();
            var validAccountNumber = "123456789";

            account.AccountNumber = validAccountNumber;

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(true));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnFalseWhenContainsUnknownNumber()
        {
            // Arrange
            var account = new BankAccount();
            var inValidAccountNumber = "123456?89";

            account.AccountNumber = inValidAccountNumber;

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(false));
        }
    }
}
