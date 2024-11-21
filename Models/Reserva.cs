public class Reserva
{
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public int RutaId { get; set; } // Agrega el ID de la Ruta
    public RutaBus Ruta { get; set; }
    public int AsientoSeleccionadoId { get; set; } // Agrega el ID del Asiento
    public Asiento AsientoSeleccionado { get; set; }
    public string EstadoPago { get; set; }
    public DateTime FechaReserva { get; set; }
    public bool PuedeCancelar
    {
        get
        {
            var fechaActual = DateTime.Now;
            var fechaSalida = Ruta.Fecha.Date.Add(Ruta.HoraSalida);
            return (fechaSalida - fechaActual).TotalHours >= 2;
        }
    }
}
