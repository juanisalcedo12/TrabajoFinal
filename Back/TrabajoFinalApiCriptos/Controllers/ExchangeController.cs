using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalApiCriptos.Data;

namespace TrabajoFinalApiCriptos.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : Controller
    {
        Context _context;

        HttpClient _http;

        public ExchangeController(Context context, HttpClient http)
        {
            _context = context;
            _http = http;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var exchanges = await _context.Exchanges
                .Select(e => new { e.Id, e.Nombre })
                .ToListAsync();

            return Ok(exchanges);
        }

    }
}
