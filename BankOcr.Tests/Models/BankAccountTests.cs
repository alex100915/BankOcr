using BankOcr.Models;
using BankOcr.Constants;
using Newtonsoft.Json;

namespace BankOcr.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void BankAccount_ToString_ShouldReturnAccountNumber()
        {
            // Arrange
            var account = new BankAccount("123456789"); // valid number
            var expectedOutput = $"{account.AccountNumber}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BankAccount_ToString_ShouldReturnAccountNumberWithIllegibleStatus()
        {
            // Arrange
            var account = new BankAccount("123444449");
            account.Status = BankAccountStatus.Illegible;

            var expectedOutput = $"{account.AccountNumber} {account.Status}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BankAccount_ToString_ShouldReturnAccountNumberWithAmbigiousStatusAndNumbers()
        {
            // Arrange
            var account = new BankAccount("123456789");
            account.Status = BankAccountStatus.Ambiguous;
            account.AmbiguousAccountNumbers = new List<string> { "123456780", "123456781" };

            var expectedOutput = $"{account.AccountNumber} {account.Status} {JsonConvert.SerializeObject(account.AmbiguousAccountNumbers)}";

            // Act
            var output = account.ToString();

            // Assert
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnFalseWhenNumberChecksumInvalid()
        {
            // Arrange
            var account = new BankAccount("123433739");

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(false));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnTrueWhenNumberChecksumValid()
        {
            // Arrange
            var account = new BankAccount("123456789");

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(true));
        }

        [Test]
        public void BankAccount_IsValid_ShouldReturnFalseWhenContainsUnknownNumber()
        {
            // Arrange
            var account = new BankAccount("123456?89");

            // Act
            var output = account.IsValid();

            // Assert
            Assert.That(output, Is.EqualTo(false));
        }
    }
}
