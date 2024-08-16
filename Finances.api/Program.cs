using Finances.api.Data;
using Finances.api.Data.Handlers;
using Finances.Core.Handler;
using Finances.Core.Handlers;
using Microsoft.EntityFrameworkCore;
using TransactionHandler = Finances.Core.Handlers.TransactionHandler;

var builder = WebApplication.CreateBuilder(args);

const string connectionString =
    "Data Source=VINHOTE-NOT\\TOTVSBD;Initial Catalog=Finance;Integrated Security=False;User ID=sa;Password=admin;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";

builder.Services.AddDbContext<AppDbContext>(
    x => x.UseSqlServer(connectionString));

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
// Server=localhost,1433;Database=Finances;User ID=sa;Password=admin;Trusted_Connection=False;TrustedServerCertificate=True;