using BankOcr.Constants;

namespace BankOcr.Models
{
    public class BankAccount
    {
        private string _accountNumber;

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                _accountNumber = value;
                ValidChecksum = IsValidChecksum(_accountNumber);
            }
        }

        public bool ValidChecksum { get; private set; }

        private bool IsValidChecksum(string accountNumber)
        {
            if (accountNumber.Length != BankAccountSettings.Length || !accountNumber.All(char.IsDigit))
            {
                return false;
            }

            var bankAccountNumbers = accountNumber
                .ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToList();

            int checkSum = 0;

            for (int i = 0; i < BankAccountSettings.Length; i++)
            {
                checkSum += (i + 1) * bankAccountNumbers[BankAccountSettings.Length - 1 - i];
            }

            return checkSum % BankAccountSettings.Checksum == 0;
        }
    }
}
