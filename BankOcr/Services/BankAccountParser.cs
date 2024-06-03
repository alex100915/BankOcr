using BankOcr.Exceptions;
using BankOcr.Constants;
using BankOcr.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class BankAccountParser
{
    public List<BankAccount> ParseFromOcr(List<OcrBankAccount> bankAccountsOcr)
    {
        List<BankAccount> bankAccounts = new List<BankAccount>();

        foreach (var bankAccountOcr in bankAccountsOcr)
        {
            BankAccount bankAccount = new BankAccount();

            var ocrNumbers = GetOcrNumbers(bankAccountOcr);
            
            string accountNumber = string.Empty;

            foreach (var ocrNumber in ocrNumbers)
            {
                accountNumber += ParseNumberFromOcr(ocrNumber);
            }

            bankAccount.AccountNumber = accountNumber;

            bankAccounts.Add(bankAccount);
        }

        return bankAccounts;

    }

    public List<string> GetOcrNumbers(OcrBankAccount bankAccountOcr)
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

    private string ParseNumberFromOcr(string ocrNumber)
    {
        var number = OcrNumbers.OcrNumbersDictionary.FirstOrDefault(n => n.Key == ocrNumber);

        return number.Key == null ? "?" : number.Value.ToString();
    }
}