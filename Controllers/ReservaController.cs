using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class ReservaController : Controller
{
    private readonly TravelContext _context;
    private readonly UserManager<Usuario> _userManager;

    public ReservaController(TravelContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Detalles(int rutaId)
    {
        var ruta = _context.RutasBuses
            .Include(r => r.Asientos)
            .FirstOrDefault(r => r.Id == rutaId);

        if (ruta != null)
        {
            // Filtra solo los asientos disponibles
            ruta.Asientos = ruta.Asientos.Where(a => a.Disponible).ToList();
        }

        return View(ruta);
    }

    [HttpPost]
    public async Task<IActionResult> Confirmar(int rutaId, List<string> asientosSeleccionados)
    {
        if (asientosSeleccionados == null || !asientosSeleccionados.Any())
        {
            TempData["Error"] = "Debe seleccionar al menos un asiento.";
            return RedirectToAction("Detalles", "Reserva", new { rutaId = rutaId });
        }

        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            TempData["Error"] = "Debe iniciar sesión para hacer una reserva.";
            return RedirectToAction("Login", "Usuario");
        }

        foreach (var numeroAsiento in asientosSeleccionados)
        {
            var asiento = await _context.Asientos
                .FirstOrDefaultAsync(a => a.Numero == numeroAsiento && a.RutaId == rutaId);

            if (asiento != null && asiento.Disponible)
            {
                asiento.Disponible = false;

                var reserva = new Reserva
                {
                    UsuarioId = userId,
                    AsientoSeleccionadoId = asiento.Id,
                    RutaId = rutaId,
                    EstadoPago = "Pagado",
                    FechaReserva = DateTime.Now
                };

                _context.Reservas.Add(reserva);
                _context.Asientos.Update(asiento); // Actualiza el asiento como no disponible
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Reserva confirmada con éxito.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CancelarReserva(int reservaId)
    {
        var reserva = await _context.Reservas
            .Include(r => r.AsientoSeleccionado)
            .Include(r => r.Ruta) // Incluye la ruta para obtener su fecha de salida
            .FirstOrDefaultAsync(r => r.Id == reservaId);

        if (reserva != null)
        {
            // Obtener la fecha y hora actual
            var fechaActual = DateTime.Now;

            // Combinar la fecha y hora de salida de la ruta
            var fechaSalida = reserva.Ruta.Fecha.Date.Add(reserva.Ruta.HoraSalida);

            // Verificar si faltan menos de 2 horas para la salida
            if ((fechaSalida - fechaActual).TotalHours < 2)
            {
                TempData["Error"] = "No se puede cancelar la reserva porque falta menos de dos horas para la salida.";
                return RedirectToAction("Historial", "Usuario");
            }

            // Marcar el asiento como disponible
            reserva.AsientoSeleccionado.Disponible = true;

            // Remover la reserva
            _context.Reservas.Remove(reserva);

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            TempData["Success"] = "Reserva cancelada exitosamente.";
        }
        else
        {
            TempData["Error"] = "No se pudo encontrar la reserva.";
        }

        // Redirigir a la acción Historial en el controlador Usuario
        return RedirectToAction("Historial", "Usuario");
    }


}
