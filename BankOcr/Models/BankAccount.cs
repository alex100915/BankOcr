using BankOcr.Constants;
using Newtonsoft.Json;

namespace BankOcr.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; private set; }

        public string Status { get; set; }

        public IEnumerable<string> AmbiguousAccountNumbers { get; set; }

        public BankAccount(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public bool IsValid() => 
            !(AccountNumber.Contains(OcrNumbers.Unknown) || !(IsValidChecksum(AccountNumber)));

        public bool IsEligibleForCorrection() =>
            AccountNumber.Count(c => c == char.Parse(OcrNumbers.Unknown)) <= 1;

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Status))
                return AccountNumber;

            if (Status == BankAccountStatus.Ambiguous)
                return string.Join(" ", AccountNumber, Status, JsonConvert.SerializeObject(AmbiguousAccountNumbers));

            return string.Join(" ", AccountNumber, Status);
        }

        private bool IsValidChecksum(string accountNumber)
        {
            if (accountNumber.Length != BankAccountSettings.Length || !accountNumber.All(char.IsDigit))
                return false;

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
