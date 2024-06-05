using BankOcr.Constants;
using BankOcr.Exceptions;
using BankOcr.Models;

namespace BankOcr.Services
{
    public class OcrBankAccountReader : IOcrBankAccountReader
    {
        public List<OcrBankAccount> Read(List<string> fileLines)
        {
            if (fileLines.Count() % OcrBankAccountSettings.LineHeight != 0)
                throw new FileLengthException("Each bank account record should have exactly 3 lines followed by 1 empty line");

            List<OcrBankAccount> bankAccounts = new List<OcrBankAccount>();

            for (int i = 0; i < fileLines.Count; i += OcrBankAccountSettings.LineHeight)
            {
                OcrBankAccount bankAccount = new OcrBankAccount
                {
                    Line1 = fileLines[i],
                    Line2 = fileLines[i + 1],
                    Line3 = fileLines[i + 2]
                };

                if (!string.IsNullOrEmpty(fileLines[i + 3]))
                    throw new FileStructureException("Each bank account should be followed by an empty line");

                if (bankAccount.Line1.Length != OcrBankAccountSettings.LineLength
                || bankAccount.Line2.Length != OcrBankAccountSettings.LineLength
                || bankAccount.Line3.Length != OcrBankAccountSettings.LineLength)
                {
                    throw new OcrBankAccountLengthException($"Each line in input file should have exactly {OcrBankAccountSettings.LineLength} characters");
                }

                bankAccounts.Add(bankAccount);               
            }

            return bankAccounts;
        }
    }
}