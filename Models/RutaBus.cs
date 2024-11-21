public class RutaBus
{
    public int Id { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraSalida { get; set; }
    public TimeSpan HoraLlegada { get; set; }
    public decimal Precio { get; set; }
    public List<Asiento> Asientos { get; set; } // Disponibilidad de asientos
    public string BusInfo { get; set; } // Información adicional sobre el autobús
}
