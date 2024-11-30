using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001; // Puerto HTTPS que configuraste
});

// Configura el DbContext para usar SQL Server
builder.Services.AddDbContext<TravelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura Identity con roles
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<TravelContext>()
    .AddDefaultTokenProviders();

// Configura servicios adicionales
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data e inicializar roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

//     // Seed roles "Usuario" y "Admin"
//     TravelContext.SeedRolesAsync(services).Wait();

//     // Seed data adicional si es necesario
//     var context = services.GetRequiredService<TravelContext>();
//     TravelContext.SeedData(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Asegúrate de agregar autenticación antes de la autorización
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();