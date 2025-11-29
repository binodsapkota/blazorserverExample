using DxDocumentControlApp.Components;
using DxDocumentControlApp.Data;
using DxDocumentControlApp.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// 1️⃣ Add services
// ------------------------

// EF Core SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MudBlazor services
builder.Services.AddMudServices();

// Document/File services
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddSingleton<IFileStorageProvider>(sp =>
    new LocalFileStorageProvider(builder.Configuration["Storage:Local:RootPath"]));

// Razor Components + Interactive Server Components
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();  // .NET 8 API

// ------------------------
// 2️⃣ Build the app
// ------------------------
var app = builder.Build();

// ------------------------
// 3️⃣ Configure middleware
// ------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// Optional: map API controllers if you have any
//app.MapControllers();

// Map static assets for Razor Components
app.MapStaticAssets();

// Map root component
app.MapRazorComponents<RootComponent>()
    .AddInteractiveServerRenderMode();

// ------------------------
// 4️⃣ Run the app
// ------------------------
app.Run();
