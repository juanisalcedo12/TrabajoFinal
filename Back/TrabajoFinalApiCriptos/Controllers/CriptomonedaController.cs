using Microsoft.AspNetCore.Mvc;
using TrabajoFinalApiCriptos.Models;
using TrabajoFinalApiCriptos.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace TrabajoFinalApiCriptos.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class CriptomonedaController : Controller
    {
        Context _context;
        HttpClient _http;


        public CriptomonedaController(Context context, HttpClient http)
        {
            _context = context;
            _http = http;

        }

        [HttpGet]
        public async Task<IActionResult> getCriptomonedas()
        {
            var cripto = await _context.CriptoMonedas
                       .Select(c => new { c.Id,c.Codigo})
                       .ToListAsync();
            return Ok(cripto);
                 
                       

        } 

    }
}
