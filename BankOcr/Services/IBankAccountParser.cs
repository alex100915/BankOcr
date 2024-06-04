using BankOcr.Models;

public interface IBankAccountParser
{
    List<BankAccount> ParseFromOcr(List<OcrBankAccount> bankAccountsOcr);
}