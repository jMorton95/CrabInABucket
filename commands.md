Entity Framework Commands:
dotnet ef migrations add [MigrationName] --startup-project=FinanceManager.Api --project=FinanceManager.Data  
dotnet ef database update [MigrationName] --startup-project=FinanceManager.Api --project=FinanceManager.Data  

