using Microsoft.AspNetCore.Identity;

public class Usuario : IdentityUser
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public List<Reserva> HistorialReservas { get; set; }
}
