using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Data;
using BookingTracker.Models;
using BookingTracker.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add API Key authentication
builder.Services.AddAuthentication()
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme,
        options => { });

// Require authentication globally for all Razor Pages
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Error");
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapControllers();

// Ensure data directory exists
var dataDirectory = builder.Configuration["DataDirectory"] ?? "/data";
if (!Directory.Exists(dataDirectory))
{
    Directory.CreateDirectory(dataDirectory);
}

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    // Create admin account if configured
    var adminEmail = builder.Configuration["ADMIN_EMAIL"];
    var adminPassword = builder.Configuration["ADMIN_PASSWORD"];

    if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Create Admin role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create or update admin user
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                ApiKey = Guid.NewGuid().ToString("N")
            };
            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            // Update password on every boot
            var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
            await userManager.ResetPasswordAsync(adminUser, token, adminPassword);

            // Ensure user is in Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Generate API key if not exists
            if (string.IsNullOrEmpty(adminUser.ApiKey))
            {
                adminUser.ApiKey = Guid.NewGuid().ToString("N");
                await userManager.UpdateAsync(adminUser);
            }
        }
    }
}

app.Run();
