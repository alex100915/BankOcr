using BankOcr.Constants;
using BankOcr.Exceptions;
using BankOcr.Models;

namespace BankOcr.Services
{
    public class OcrBankAccountReader
    {
        public List<OcrBankAccount> Read(List<string> fileLines)
        {
            if (fileLines.Count() % OcrBankAccountSettings.BankAccountHeight != 0)
                throw new FileLengthException("Each bank account record should have exactly 3 lines followed by 1 empty line");

            List<OcrBankAccount> bankAccounts = new List<OcrBankAccount>();

            for (int i = 0; i < fileLines.Count; i += OcrBankAccountSettings.BankAccountHeight)
            {
                bankAccounts.Add(new OcrBankAccount
                {
                    Line1 = fileLines[i],
                    Line2 = fileLines[i + 1],
                    Line3 = fileLines[i + 2]
                });

                if (!string.IsNullOrEmpty(fileLines[i + 3]))
                    throw new FileStructureException("Each bank account should be followed by an empty line");
            }

            return bankAccounts;
        }
    }
}