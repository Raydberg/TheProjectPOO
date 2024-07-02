namespace CapaPresentacionTienda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configuración para utilizar sesiones
            builder.Services.AddDistributedMemoryCache(); // Utiliza la caché en memoria para almacenar las sesiones.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de vida de la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configuración de autenticación basada en cookies
            builder.Services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config =>
                {
                    config.Cookie.Name = "UserLoginCookie"; // Nombre de la cookie
                    config.LoginPath = "/Acceso/Index"; // Ruta al formulario de login
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiración de la cookie
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Asegúrate de añadir esto
            app.UseAuthorization();

            // Asegúrate de llamar a UseSession() después de UseRouting() y antes de UseEndpoints().
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Tienda}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
