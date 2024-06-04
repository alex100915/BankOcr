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

            BankAccount bankAccount = new BankAccount(string.Concat(ocrNumbers.Select(ParseOcrNumber)));

            if (!bankAccount.IsValid())
            {
                if (!bankAccount.IsEligibleForCorrection())
                    bankAccount.Status = BankAccountStatus.Illegible;
                else
                    bankAccount = CorrectAccountNumber(ocrNumbers, bankAccount);
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

    private BankAccount CorrectAccountNumber(List<string> ocrNumbers, BankAccount bankAccount)
    {
        List<BankAccount> correctedBankAccount = GetCorrectedBankAccount(ocrNumbers);

        if (correctedBankAccount.Count() == 0)
        {
            bankAccount.Status = BankAccountStatus.Illegible;
        }
        else if (correctedBankAccount.Count() == 1)
        {
            bankAccount.Status = BankAccountStatus.Valid;
            bankAccount = correctedBankAccount[0];
        }
        else
        {
            bankAccount.Status = BankAccountStatus.Ambiguous;
            bankAccount.AmbiguousAccountNumbers = correctedBankAccount.Select(b => b.AccountNumber);
        }

        return bankAccount;
    }

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

                        var correctedOcrNumbers = ocrNumbers.ToList();
                        correctedOcrNumbers[ocrNumberIndex] = correctedNumber;

                        BankAccount bankAccount = new BankAccount(string.Concat(correctedOcrNumbers.Select(ParseOcrNumber)));

                        if (bankAccount.IsValid())
                            correctedBankAccounts.Add(bankAccount);
                    }
                }
            }
        }

        return correctedBankAccounts;
    }
}