using BankOcr.Models;
using BankOcr.Services;

string filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "bankAccounts.txt");
    
FileReader fileReader = new FileReader();
List<string> fileLines = fileReader.ReadFile(filePath);

OcrBankAccountReader ocrBankAccountsReader = new OcrBankAccountReader();   
List<OcrBankAccount> ocrBankAccounts = ocrBankAccountsReader.Read(fileLines);

BankAccountParser bankAccountParser = new BankAccountParser();
List<BankAccount> bankAccounts = bankAccountParser.ParseFromOcr(ocrBankAccounts);

foreach (var bankAccount in bankAccounts)
    Console.WriteLine(bankAccount.AccountNumber);