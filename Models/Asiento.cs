public class Asiento
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public bool Disponible { get; set; }
    public int RutaId { get; set; } // Agrega la referencia de la Ruta
    public RutaBus Ruta { get; set; } // Relación con la Ruta
}
