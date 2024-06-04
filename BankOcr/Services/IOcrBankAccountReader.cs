using BankOcr.Models;

namespace BankOcr.Services
{
    public interface IOcrBankAccountReader
    {
        List<OcrBankAccount> Read(List<string> fileLines);
    }
}