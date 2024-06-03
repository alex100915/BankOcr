using BankOcr.Models;

namespace BankOcr.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void AccountNumber_WithValidChecksum_SetsValidChecksumToTrue()
        {
            // Arrange
            var bankAccount = new BankAccount();

            // Act
            bankAccount.AccountNumber = "123456789"; // Example of a valid account number

            // Assert
            Assert.IsTrue(bankAccount.ValidChecksum);
        }

        [Test]
        public void AccountNumber_WithInvalidChecksum_SetsValidChecksumToFalse()
        {
            // Arrange
            var bankAccount = new BankAccount();

            // Act
            bankAccount.AccountNumber = "123456780"; // Example of an invalid account number

            // Assert
            Assert.IsFalse(bankAccount.ValidChecksum);
        }

        [Test]
        public void AccountNumber_WithIncorrectLength_SetsValidChecksumToFalse()
        {
            // Arrange
            var bankAccount = new BankAccount();

            // Act
            bankAccount.AccountNumber = "12345678"; // Example of an account number with incorrect length

            // Assert
            Assert.IsFalse(bankAccount.ValidChecksum);
        }

        [Test]
        public void AccountNumber_WithNonDigitCharacters_SetsValidChecksumToFalse()
        {
            // Arrange
            var bankAccount = new BankAccount();

            // Act
            bankAccount.AccountNumber = "12345678a"; // Example of an account number with non-digit characters

            // Assert
            Assert.IsFalse(bankAccount.ValidChecksum);
        }
    }
}
