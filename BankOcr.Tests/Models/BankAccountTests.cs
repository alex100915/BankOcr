using BankOcr.Models;
using BankOcr.Constants;

namespace BankOcr.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
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
    }
}
