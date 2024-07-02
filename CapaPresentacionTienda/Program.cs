namespace CapaPresentacionTienda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configuraci�n para utilizar sesiones
            builder.Services.AddDistributedMemoryCache(); // Utiliza la cach� en memoria para almacenar las sesiones.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de vida de la sesi�n
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Aseg�rate de llamar a UseSession() despu�s de UseRouting() y antes de UseEndpoints().
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acceso}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
