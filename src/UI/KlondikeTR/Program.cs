using KlondikeTR.Interfaces;
using KlondikeTR.Services;


var builder = WebApplication.CreateBuilder(args);


#region Add services 

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMemoryCache();

builder.Services.AddTransient<IItemsService, ItemsService>();

builder.Services.AddHttpClient("items", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ItemsApiUrl"]);
});
#endregion


var app = builder.Build();


#region Configure pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
#endregion


app.Run();
