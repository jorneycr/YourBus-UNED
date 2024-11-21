using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


public class TravelContext : IdentityDbContext<Usuario>
{
    public TravelContext(DbContextOptions<TravelContext> options) : base(options) { }

    // Define DbSet para cada modelo de la aplicación
    public DbSet<RutaBus> RutasBuses { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Asiento> Asientos { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la relación Usuario-Reserva (Uno a Muchos)
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.HistorialReservas)
            .HasForeignKey(r => r.UsuarioId) // Especificar clave foránea
            .IsRequired(); // Hace que el usuario sea obligatorio para una reserva

        // Configuración de la relación Reserva-RutaBus (Uno a Muchos)
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Ruta)
            .WithMany() // Opcional: puedes agregar un ICollection<Reserva> en RutaBus si necesitas acceder a las reservas desde la ruta
            .HasForeignKey(r => r.RutaId) // Clave externa en Reserva
            .IsRequired();

        // Configuración de la relación RutaBus-Asiento (Uno a Muchos)
        modelBuilder.Entity<Asiento>()
            .HasOne(a => a.Ruta) // Relación hacia RutaBus
            .WithMany(r => r.Asientos)
            .HasForeignKey(a => a.RutaId)
            .OnDelete(DeleteBehavior.NoAction); // Clave externa en Asiento

        // Configuración de la relación Reserva-Asiento (Uno a Uno)
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.AsientoSeleccionado)
            .WithOne()
            .HasForeignKey<Reserva>(r => r.AsientoSeleccionadoId); // Clave externa en Reserva

        // Configuración de precisión para la propiedad Precio de RutaBus
        modelBuilder.Entity<RutaBus>()
            .Property(r => r.Precio)
            .HasPrecision(18, 2);
    }

    public static void SeedData(TravelContext context)
    {
        // Asegúrate de que no hay datos en la tabla antes de agregar los nuevos
        if (!context.RutasBuses.Any())
        {
            context.RutasBuses.AddRange(
                new RutaBus
                {
                    Origen = "Ciudad A",
                    Destino = "Ciudad B",
                    Fecha = DateTime.Now.AddDays(1),
                    HoraSalida = new TimeSpan(8, 0, 0),
                    HoraLlegada = new TimeSpan(12, 30, 0),
                    Precio = 50.00m,
                    BusInfo = "Autobús Expreso",
                    Asientos = Enumerable.Range(1, 10).Select(i =>
                        new Asiento { Numero = $"1{i}A", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad A",
                    Destino = "Ciudad B",
                    Fecha = DateTime.Now.AddDays(1),
                    HoraSalida = new TimeSpan(14, 0, 0),
                    HoraLlegada = new TimeSpan(18, 30, 0),
                    Precio = 55.00m,
                    BusInfo = "Autobús Expreso",
                    Asientos = Enumerable.Range(1, 10).Select(i =>
                        new Asiento { Numero = $"2{i}A", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad B",
                    Destino = "Ciudad C",
                    Fecha = DateTime.Now.AddDays(2),
                    HoraSalida = new TimeSpan(7, 0, 0),
                    HoraLlegada = new TimeSpan(11, 30, 0),
                    Precio = 40.00m,
                    BusInfo = "Autobús Ejecutivo",
                    Asientos = Enumerable.Range(1, 15).Select(i =>
                        new Asiento { Numero = $"1{i}B", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad B",
                    Destino = "Ciudad C",
                    Fecha = DateTime.Now.AddDays(2),
                    HoraSalida = new TimeSpan(17, 0, 0),
                    HoraLlegada = new TimeSpan(21, 30, 0),
                    Precio = 42.00m,
                    BusInfo = "Autobús Ejecutivo",
                    Asientos = Enumerable.Range(1, 15).Select(i =>
                        new Asiento { Numero = $"2{i}B", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad C",
                    Destino = "Ciudad A",
                    Fecha = DateTime.Now.AddDays(3),
                    HoraSalida = new TimeSpan(10, 30, 0),
                    HoraLlegada = new TimeSpan(15, 0, 0),
                    Precio = 60.00m,
                    BusInfo = "Autobús Clásico",
                    Asientos = Enumerable.Range(1, 20).Select(i =>
                        new Asiento { Numero = $"1{i}C", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad C",
                    Destino = "Ciudad A",
                    Fecha = DateTime.Now.AddDays(4),
                    HoraSalida = new TimeSpan(13, 0, 0),
                    HoraLlegada = new TimeSpan(18, 0, 0),
                    Precio = 62.00m,
                    BusInfo = "Autobús Clásico",
                    Asientos = Enumerable.Range(1, 20).Select(i =>
                        new Asiento { Numero = $"2{i}C", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad A",
                    Destino = "Ciudad C",
                    Fecha = DateTime.Now.AddDays(5),
                    HoraSalida = new TimeSpan(9, 0, 0),
                    HoraLlegada = new TimeSpan(13, 30, 0),
                    Precio = 70.00m,
                    BusInfo = "Autobús Premium",
                    Asientos = Enumerable.Range(1, 25).Select(i =>
                        new Asiento { Numero = $"1{i}D", Disponible = true }).ToList()
                },
                new RutaBus
                {
                    Origen = "Ciudad A",
                    Destino = "Ciudad C",
                    Fecha = DateTime.Now.AddDays(5),
                    HoraSalida = new TimeSpan(15, 0, 0),
                    HoraLlegada = new TimeSpan(19, 30, 0),
                    Precio = 72.00m,
                    BusInfo = "Autobús Premium",
                    Asientos = Enumerable.Range(1, 25).Select(i =>
                        new Asiento { Numero = $"2{i}D", Disponible = true }).ToList()
                }
            );

            context.SaveChanges(); // Guardar los cambios en la base de datos
        }

    }

    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Crear rol "Usuario" si no existe
    if (!await roleManager.RoleExistsAsync("Usuario"))
    {
        await roleManager.CreateAsync(new IdentityRole("Usuario"));
    }

    // Crear rol "Admin" si no existe
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
}

}

