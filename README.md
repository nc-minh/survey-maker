# survey-swift

Provides simple online survey services

dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package MySql.Data.EntityFramework
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.Extensions.DependencyInjection

dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet aspnet-codegenerator controller -name Contact -namespace SurveyMaker.Controllers -m SurveyMaker.Models.Contact -udl -dc SurveyMaker.Models.ApplicationDbContext -outDir Controllers/

# Migration

dotnet ef migrations add Init
