using Dotnet.Playground.SignalR.Survey.Components;
using Dotnet.Playground.SignalR.Survey.Database;
using Dotnet.Playground.SignalR.Survey.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR();
builder.Services.AddDbContext<SurveyContext>(options =>
{
    options.UseSqlServer("Server=localhost,1433;Database=surveys;User ID=sa;Password=P@ssword123;Trust Server Certificate=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<CreateSurveyHub>("/createsurveys");
app.MapHub<SurveyHub>("/surveys");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
