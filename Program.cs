using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Gavre_Natanael_Lab2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Books");
    options.Conventions.AllowAnonymousToPage("/Books/Index");
    options.Conventions.AllowAnonymousToPage("/Books/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Publishers", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Categories", "AdminPolicy");
});

builder.Services.AddDbContext<Gavre_Natanael_Lab2Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Gavre_Natanael_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Gavre_Natanael_Lab2Context' not found.")));

builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Gavre_Natanael_Lab2Context")
        ?? throw new InvalidOperationException(
            "Connection string 'Gavre_Natanael_Lab2Context' not found.")));


builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
