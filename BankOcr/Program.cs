using BankOcr.Models;
using BankOcr.Services;

string filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "bankAccounts.txt");

IFileReader fileReader = new FileReader();
List<string> fileLines = fileReader.ReadFile(filePath);

IOcrBankAccountReader ocrBankAccountsReader = new OcrBankAccountReader();
List<OcrBankAccount> ocrBankAccounts = ocrBankAccountsReader.Read(fileLines);

IBankAccountParser bankAccountParser = new BankAccountParser();
List<BankAccount> bankAccounts = bankAccountParser.ParseFromOcr(ocrBankAccounts);

Console.WriteLine("Valid");
foreach (var bankAccount in bankAccounts.Where(a => a.ValidChecksum == true))
    Console.WriteLine(bankAccount.AccountNumber);

Console.WriteLine("Invalid");
foreach (var bankAccount in bankAccounts.Where(a => a.ValidChecksum == false))
    Console.WriteLine(bankAccount.AccountNumber);