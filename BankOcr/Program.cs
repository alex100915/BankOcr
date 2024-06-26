﻿using BankOcr.Models;
using BankOcr.Services;

string filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "bankAccounts.txt");
    
IFileReader fileReader = new FileReader();
List<string> fileLines = await fileReader.ReadFileAsync(filePath);

IOcrBankAccountReader ocrBankAccountsReader = new OcrBankAccountReader();   
List<OcrBankAccount> ocrBankAccounts = ocrBankAccountsReader.Read(fileLines);

IBankAccountParser bankAccountParser = new BankAccountParser();
List<BankAccount> bankAccounts = bankAccountParser.ParseFromOcr(ocrBankAccounts);

foreach (BankAccount bankAccount in bankAccounts)
    Console.WriteLine(bankAccount.ToString());