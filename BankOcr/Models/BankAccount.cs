using BankOcr.Constants;

namespace BankOcr.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }

        public string Status { get;  set; }

        public string Ambiguity { get; set; }

        public bool IsValid()
        {
            if (AccountNumber.Contains(OcrNumbers.Unknown) || !(IsValidChecksum(AccountNumber)))
                return false;

            return true;
        }

        private static bool IsValidChecksum(string accountNumber)
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

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Status))
                return AccountNumber;

            if (Status == BankAccountStatus.Ambiguous)
                return string.Join(" ", AccountNumber, Status, Ambiguity);

            return string.Join(" ", AccountNumber, Status);
        }
    }
}
