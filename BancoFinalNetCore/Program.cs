using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Entidades;
using BancoFinalNetCore.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión del DbContext desde el archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Añadir el DbContext como servicio
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IUsuarioServicio, UsuarioServicioImpl>();
builder.Services.AddScoped<IServicioEncriptar, ServicioEncriptarImpl>();
builder.Services.AddScoped<IConvertirAdto, ConvertirAdtoImpl>();
builder.Services.AddScoped<IConvertirAdao, ConvertirAdaoImpl>();
builder.Services.AddScoped<IServicioEmail, ServicioEmailImpl>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
