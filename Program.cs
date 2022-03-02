using Chat_Application.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using Chat_Application.Hubs;
using Chat_Application.Models;
using Chat_Application.Crud;
using Microsoft.Extensions.Options;
using Chat_Application.Help_Classes;
using Microsoft.AspNetCore.Components.Authorization;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();

builder.Services.Configure<DBElements>(
    builder.Configuration.GetSection(nameof(DBElements)));
builder.Services.AddSingleton<IDBElements>(x =>
    x.GetRequiredService<IOptions<DBElements>>().Value);

builder.Services.AddBlazoredLocalStorage();


builder.Services.AddSingleton<UserCrud>();
builder.Services.AddSingleton<ConnectionCrud>();

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.MapDefaultControllerRoute();
}

app.UseHttpsRedirection();



app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();

app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



