using BankOcr.Exceptions;
using BankOcr.Constants;
using BankOcr.Models;
using Newtonsoft.Json;

public class BankAccountParser : IBankAccountParser
{
    public List<BankAccount> ParseFromOcr(List<OcrBankAccount> bankAccountsOcr)
    {
        List<BankAccount> bankAccounts = new List<BankAccount>();

        foreach (var bankAccountOcr in bankAccountsOcr)
        {
            var ocrNumbers = GetOcrNumbers(bankAccountOcr);

            BankAccount bankAccount = new BankAccount
            {
                AccountNumber = string.Concat(ocrNumbers.Select(ParseOcrNumber))
            };

            if (bankAccount.HasProblem())
            {
                if (!IsEligibleForCorrection(bankAccount))
                {
                    bankAccounts.Add(bankAccount);
                    continue;
                }

                List<BankAccount> correctedBankAccount = GetCorrectedBankAccount(ocrNumbers);

                if (correctedBankAccount.Count() == 1)
                    bankAccount = correctedBankAccount[0];
                else if (correctedBankAccount.Count() == 0)
                    bankAccount.Status = BankAccountStatus.Illegible;
                else
                    bankAccount.Ambiguity = JsonConvert.SerializeObject(correctedBankAccount.Select(b => b.AccountNumber));
            }

            bankAccounts.Add(bankAccount);
        }

        return bankAccounts;

    }

    private List<string> GetOcrNumbers(OcrBankAccount bankAccountOcr)
    {
        ValidateOcrBankAccount(bankAccountOcr);

        var ocrNumbers = new List<string>();

        for (int i = 0; i < OcrBankAccountSettings.BankAccountLength; i += OcrBankAccountSettings.DigitLength)
        {
            string ocrNumber = bankAccountOcr.Line1.Substring(i, OcrBankAccountSettings.DigitLength);
            ocrNumber += bankAccountOcr.Line2.Substring(i, OcrBankAccountSettings.DigitLength);
            ocrNumber += bankAccountOcr.Line3.Substring(i, OcrBankAccountSettings.DigitLength);

            ocrNumbers.Add(ocrNumber);
        }

        return ocrNumbers;
    }

    private void ValidateOcrBankAccount(OcrBankAccount ocrBankAccount)
    {
        if (ocrBankAccount.Line1.Length != OcrBankAccountSettings.BankAccountLength
            || ocrBankAccount.Line2.Length != OcrBankAccountSettings.BankAccountLength
            || ocrBankAccount.Line3.Length != OcrBankAccountSettings.BankAccountLength)
        {
            throw new BankAccountLengthException($"Each line in input file should have exactly {OcrBankAccountSettings.BankAccountLength} characters");
        }
    }

    private string ParseOcrNumber(string ocrNumber)
    {
        return OcrNumbers.OcrNumbersDictionary.TryGetValue(ocrNumber, out var number)
            ? number.ToString()
            : OcrNumbers.Unknown;
    }

    private bool IsEligibleForCorrection(BankAccount bankAccount) =>
        bankAccount.AccountNumber.Count(c => c == char.Parse(OcrNumbers.Unknown)) <= 1;

    private List<BankAccount> GetCorrectedBankAccount(List<string> ocrNumbers)
    {
        var replacements = new List<Tuple<char, char>>
        {
            new Tuple<char, char>(' ', '|'),
            new Tuple<char, char>(' ', '_'),
            new Tuple<char, char>('|', ' '),
            new Tuple<char, char>('_', ' ')
        };

        List<BankAccount> correctedBankAccounts = new List<BankAccount>();

        foreach (var replacement in replacements)
        {
            for (int ocrNumberIndex = 0; ocrNumberIndex < ocrNumbers.Count; ocrNumberIndex++)
            {
                var ocrNumber = ocrNumbers[ocrNumberIndex];

                for (int charIndex = 0; charIndex < ocrNumber.Length; charIndex++)
                {
                    if (ocrNumber[charIndex] == replacement.Item1)
                    {
                        string correctedNumber = ocrNumber.Substring(0, charIndex) + replacement.Item2 + ocrNumber.Substring(charIndex + 1);

                        if (ParseOcrNumber(correctedNumber) == OcrNumbers.Unknown)
                            continue;

                        var correctedNumbers = ocrNumbers.ToList();
                        correctedNumbers[ocrNumberIndex] = correctedNumber;

                        BankAccount bankAccount = new BankAccount()
                        {
                            AccountNumber = string.Concat(correctedNumbers.Select(ParseOcrNumber))
                        };

                        if (!bankAccount.HasProblem())
                            correctedBankAccounts.Add(bankAccount);
                    }
                }
            }
        }

        return correctedBankAccounts;
    }
}