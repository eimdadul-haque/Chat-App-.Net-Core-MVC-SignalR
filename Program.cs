using Chat_App_.Net_Core_MVC_SignalR.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configure = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configure.GetConnectionString("Dev")));
builder.Services.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 4;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
});

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chat");

app.Run();
