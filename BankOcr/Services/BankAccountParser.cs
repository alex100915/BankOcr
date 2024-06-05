﻿using BankOcr.Constants;
using BankOcr.Models;

public class BankAccountParser : IBankAccountParser
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

    private List<string> GetOcrNumbers(OcrBankAccount bankAccountOcr)
    {
        var ocrNumbers = new List<string>();

        for (int i = 0; i < OcrBankAccountSettings.LineLength; i += OcrBankAccountSettings.DigitLength)
        {
            string ocrNumber = bankAccountOcr.Line1.Substring(i, OcrBankAccountSettings.DigitLength);
            ocrNumber += bankAccountOcr.Line2.Substring(i, OcrBankAccountSettings.DigitLength);
            ocrNumber += bankAccountOcr.Line3.Substring(i, OcrBankAccountSettings.DigitLength);

            ocrNumbers.Add(ocrNumber);
        }

        return ocrNumbers;
    }

    private string ParseNumberFromOcr(string ocrNumber)
    {
        var number = OcrNumbers.OcrNumbersDictionary.FirstOrDefault(n => n.Key == ocrNumber);

        return number.Key == null ? OcrNumbers.Unknown : number.Value.ToString();
    }
}