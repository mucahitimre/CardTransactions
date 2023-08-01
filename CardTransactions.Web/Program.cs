using CardTransactions.Api.Abstractions;
using CardTransactions.Domain.Abstractions;
using CardTransactions.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IMongoService, MongoService>();
builder.Services.AddTransient<ISalesManager, SalesManager>();
builder.Services.AddTransient<IBankService, BankService>();

var app = builder.Build();

// Create indexes
app.Services.GetService<IMongoService>().CreateIndexAsync();
await app.Services.GetService<IMongoService>().CreateDumyDateAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
