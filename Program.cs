using Microsoft.AspNetCore.Http.Connections;
using Server_Dotnet.Pages.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CustomSignalrAuth", policy =>
        {
            policy.Requirements.Add(new CustomSignalrAuthRequirement());
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseWebSockets();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    HubMappingExtension.MapHubs(endpoints);
});
#pragma warning restore ASP0014

app.Run();
