using SkillCentral.ApiClients;
using SkillCentral.Client.Pages;
using SkillCentral.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddHttpClient<IEmployeeHttpClient, EmployeeHttpClient>(client => client.BaseAddress = new("http://employeeservices"));
//builder.Services.AddHttpClient<ISkillHttpClient, SkillHttpClient>(client => client.BaseAddress = new("http://skillservices"));
//builder.Services.AddHttpClient<INotificationHttpClient, NotificationHttpClient>(client => client.BaseAddress = new("http://notificationservices"));

builder.Services.AddHttpClient<IEmployeeHttpClient, EmployeeHttpClient>(client => client.BaseAddress = new("http://localhost:5228"));
builder.Services.AddHttpClient<ISkillHttpClient, SkillHttpClient>(client => client.BaseAddress = new("http://localhost:5203"));
builder.Services.AddHttpClient<INotificationHttpClient, NotificationHttpClient>(client => client.BaseAddress = new("http://localhost:5013"));

builder.Services.AddMudServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
