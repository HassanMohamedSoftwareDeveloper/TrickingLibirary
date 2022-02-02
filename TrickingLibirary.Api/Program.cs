using TrickingLibirary.Api.Extensions;
using TrickingLibirary.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services
    .RegisterWebServices()
    .RegisterDatabase()
    .RegisterVideoManagerServices()
    .RegisterIdentityServices(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("All");
app.UseAuthentication();
app.UseIdentityServer();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

TestDataHelper.AddTestData(app.Services, app.Environment);

app.Run();
