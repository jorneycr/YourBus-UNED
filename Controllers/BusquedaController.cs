using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BusquedaController : Controller
{
    private readonly TravelContext _context;

    public BusquedaController(TravelContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Resultados(string origen, string destino, DateTime fecha)
    {
        var rutas = await _context.RutasBuses
            .Where(r => r.Origen == origen && r.Destino == destino && r.Fecha.Date == fecha.Date)
            .Include(r => r.Asientos)
            .ToListAsync();

        return View(rutas);
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int rutaId)
    {
        var ruta = await _context.RutasBuses
            .Include(r => r.Asientos)
            .FirstOrDefaultAsync(r => r.Id == rutaId);

        if (ruta == null)
            return NotFound();

        return View(ruta);
    }
    public IActionResult Index()
    {
        return View();
    }
}
