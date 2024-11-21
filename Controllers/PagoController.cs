using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PagoController : Controller
{
    private readonly TravelContext _context;
    private readonly UserManager<Usuario> _userManager;

    public PagoController(TravelContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public IActionResult ProcesarPago(int rutaId, string asientosSeleccionados)
    {
        // Mantén los asientos seleccionados como string sin convertir a int
        var asientos = asientosSeleccionados.Split(',').ToList();

        var ruta = _context.RutasBuses.Find(rutaId);
        if (ruta == null) return NotFound();

        ViewBag.RutaId = rutaId;
        ViewBag.AsientosSeleccionados = asientos;

        // Redirige a la vista de pago con la información necesaria
        return View("ProcesarPago");
    }

    [HttpPost]
public IActionResult ConfirmarPago(int rutaId, string asientosSeleccionados, string cardNumber, string expiryDate, string cvv)
{
    // Simulación de pago
    if (cardNumber.Length == 16 && expiryDate.Length == 5 && cvv.Length == 3)
    {
        var asientos = asientosSeleccionados.Split(',').ToList();
        var ruta = _context.RutasBuses.Include(r => r.Asientos).FirstOrDefault(r => r.Id == rutaId);

        if (ruta != null)
        {
            var userId = _userManager.GetUserId(User);
            var usuario = _context.Usuarios.Find(userId);

            if (userId == null || usuario == null)
            {
                TempData["Error"] = "Debe iniciar sesión para hacer una reserva.";
                return RedirectToAction("Login", "Usuario");
            }

            string recibo = Guid.NewGuid().ToString(); // Generar recibo único
            var detallesAsientos = new List<string>();

            foreach (var asiento in ruta.Asientos.Where(a => asientos.Contains(a.Numero)))
            {
                asiento.Disponible = false;
                detallesAsientos.Add(asiento.Numero);

                var reserva = new Reserva
                {
                    UsuarioId = userId,
                    AsientoSeleccionadoId = asiento.Id,
                    RutaId = rutaId,
                    EstadoPago = "Pagado",
                    FechaReserva = DateTime.Now
                };
                _context.Reservas.Add(reserva);
            }

            _context.SaveChanges();

            // Pasar información detallada a la vista
            ViewBag.Recibo = recibo;
            ViewBag.FechaPago = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.EstadoPago = "Pagado";
            ViewBag.UsuarioNombre = usuario.Nombre;
            ViewBag.UsuarioEmail = usuario.Email;
            ViewBag.Asientos = string.Join(", ", detallesAsientos);
            ViewBag.RutaNombre = ruta.BusInfo; // O cualquier otro detalle relevante de la ruta

            return PartialView("_ConfirmacionPagoModal");
        }
    }

    //TempData["Error"] = "Error en el procesamiento del pago. Inténtelo nuevamente.";
    return RedirectToAction("Detalles", new { rutaId });
}


}
