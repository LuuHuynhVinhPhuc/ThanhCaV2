using ThanhCaV2.Application;
using ThanhCaV2.Infrastructure;
using ThanhCaV2.Blazor.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ThanhCaV2.Infrastructure.Persistances;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/data/keys"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ThanhCaV2.Blazor.Services.ToastService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
