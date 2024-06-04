using BankOcr.Constants;

namespace BankOcr.Models
{
    public class BankAccount
    {
        private string _accountNumber;
        private string ambiguity;

        public string AccountNumber
        {
            get => _accountNumber;

            set
            {
                _accountNumber = value;
                Status = CheckStatus(_accountNumber);
            }
        }

        public string Status { get;  set; }

        public string Ambiguity
        {
            get => ambiguity;
            
            set
            {
                ambiguity = value;
                Status = BankAccountStatus.Ambiguous;
            }
        }

        public bool IsValidChecksum(string accountNumber)
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

        public string CheckStatus(string accountNumber)
        {            
            if (accountNumber.Contains(OcrNumbers.Unknown))
                return BankAccountStatus.Illegible;

            if (!IsValidChecksum(accountNumber))
                return BankAccountStatus.ChecksumInvalid;

            return string.Empty;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Status))
                return AccountNumber;

            if (Status == BankAccountStatus.Ambiguous)
                return string.Join(" ", AccountNumber, Status, Ambiguity);

            return string.Join(" ", AccountNumber, Status);
        }

        public bool HasProblem() => !string.IsNullOrEmpty(Status);
    }
}
