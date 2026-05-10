using BuildertrendMVC.Middleware;
using BuildertrendMVC.Services;
using Microsoft.Extensions.Configuration;
using BuildertrendMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// Email settings
// Configuración de EmailService
var builder = WebApplication.CreateBuilder(args);
// No registrar UserCultureMiddleware como servicio, solo usarlo en el pipeline
builder.Services.AddSingleton(provider => {
    var config = provider.GetRequiredService<IConfiguration>();
    var emailSettings = new BuildertrendMVC.Models.EmailSettings();
    config.GetSection("EmailSettings").Bind(emailSettings);
    return new EmailService(emailSettings.SmtpHost, emailSettings.SmtpPort, emailSettings.SmtpUser, emailSettings.SmtpPass, emailSettings.From);
});



// Configurar EF Core con SQLite para datos de la app
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));




// --- LOCALIZACIÓN ---

// Identity y autenticación
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAuthentication();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "es", "en" };
    options.SetDefaultCulture("es");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BuildertrendMVC.Services.AuditService>();
builder.Services.AddScoped<BuildertrendMVC.Services.IPermissionService, BuildertrendMVC.Services.PermissionService>();

// Registro de DummyEmailSender para desarrollo
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, DummyEmailSender>();

var app = builder.Build();

// Seed de permisos y roles base
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    var permissionService = services.GetRequiredService<IPermissionService>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    
    await PermissionSeeder.SeedAsync(db, roleManager, permissionService);
    await AdminSeeder.SeedAdminAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();


app.UseRouting();

// Middleware para establecer cultura según usuario
app.Use(async (context, next) =>
{
    var userCulture = context.RequestServices.GetService<UserCultureMiddleware>();
    if (userCulture != null)
        await userCulture.InvokeAsync(context, context.RequestServices.GetService<UserManager<ApplicationUser>>());
    else
        await next();
});

// Middleware de localización
var locOptions = app.Services.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions?.Value);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Endpoints de Identity UI
app.MapRazorPages();

app.Run();
