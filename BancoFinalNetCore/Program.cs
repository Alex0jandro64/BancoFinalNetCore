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
builder.Services.AddScoped<ICuentaServicio, ServicioCuentaImpl>();
builder.Services.AddScoped<ITransaccionServicio, TransaccionServicioImpl>();
builder.Services.AddScoped<IOficinaServicio, OficinaServicioImpl>();
builder.Services.AddScoped<ICitaServicio, CitaServicioImpl>();


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


// Método para inicializar el usuario administrador
void InicializarUsuarioAdmin()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        var servicioEncriptar = scope.ServiceProvider.GetRequiredService<IServicioEncriptar>(); // Obtener el servicio de encriptación

        // Verificar si el usuario admin ya existe
        if (!dbContext.Usuarios.Any(u => u.EmailUsuario == "admin@admin.com"))
        {
            // Crear el usuario admin y otros datos asociados
            var admin = new Usuario
            {
                NombreUsuario = "admin",
                ApellidosUsuario = "admin",
                TlfUsuario = "admin",
                ClaveUsuario = "admin", // Considera encriptar la contraseña
                DniUsuario = "-",
                EmailUsuario = "admin@admin.com",
                Rol = "ROLE_ADMIN",
                MailConfirmado = true
            };

            admin.ClaveUsuario = servicioEncriptar.Encriptar(admin.ClaveUsuario);

            var cuentaAdmin = new CuentaBancaria
            {
                UsuarioCuenta = admin,
                SaldoCuenta = 120.50m,
                CodigoIban = "1234"
            };

            var oficina = new Oficina
            {
                DireccionOficina = "Calle Pepe Nº2"
            };


            dbContext.Oficinas.Add(oficina);
            dbContext.Usuarios.Add(admin);
            dbContext.CuentasBancarias.Add(cuentaAdmin);
            dbContext.SaveChanges(); // Guardar los cambios en la base de datos
        }
    }
}


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

InicializarUsuarioAdmin();
app.Run();

