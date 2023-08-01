using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Abstractions;
using CardTransactions.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMongoService, MongoService>();
builder.Services.AddTransient<ISalesManager, SalesManager>();
builder.Services.AddTransient<IBankService, BankService>();

var app = builder.Build();

// Create indexes
app.Services.GetService<IMongoService>().CreateIndexAsync();
await app.Services.GetService<IMongoService>().CreateDumyDateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

