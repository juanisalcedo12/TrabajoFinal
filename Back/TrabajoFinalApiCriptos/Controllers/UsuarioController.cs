using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalApiCriptos.Data;
using TrabajoFinalApiCriptos.Models;
using TrabajoFinalApiCriptos.Models.Dtos_Views;

namespace TrabajoFinalApiCriptos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        Context _context;

        HttpClient _http;

        public UsuarioController(Context context, HttpClient http)
        {
            _context = context;
            _http = http;
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuarios([FromBody] UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { error = "Ya existe un usuario con ese email." });


            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                SaldoARS = dto.SaldoARS
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(new { usuario.Id, usuario.Nombre, usuario.Email, usuario.SaldoARS });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuario = await _context.Usuarios
                .Select(u => new { u.Id, u.Nombre })
                .ToListAsync();

            return Ok(usuario);
        }

        [HttpGet("wallet-status/{usuarioId}")]
        public async Task<IActionResult> GetWalletStatus(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            // Obtenemos las criptomonedas que el usuario posee, agrupadas
            var transacciones = await _context.Transacciones
                .Include(t => t.Criptomoneda)
                .Include(t => t.Exchange)
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();

            var saldos = transacciones
                .GroupBy(t => t.Criptomoneda.Codigo)
                .Select(g =>
                {
                    var cantidad = g.Sum(t => t.TipoAccion == "purchase" ? t.CantidadCripto : -t.CantidadCripto);
                    var ultimaTx = g.OrderByDescending(t => t.FechaHora).First(); // Última transacción
                    return new
                    {
                        CryptoCode = g.Key,
                        Cantidad = cantidad,
                        ExchangeNombre = ultimaTx.Exchange.Nombre
                    };
                })
                .Where(x => x.Cantidad > 0)
                .ToList();

            var resultado = new EstadoWalletDto
            {
                UsuarioId = usuarioId,
                SaldoARS = usuario.SaldoARS,
                Monedas = new List<EstadoCriptoDto>(),
                TotalARS = 0
            };

            foreach (var saldo in saldos)
            {
                var url = $"https://criptoya.com/api/{saldo.ExchangeNombre.ToLower()}/{saldo.CryptoCode.ToLower()}/ars";
                var precio = await _http.GetFromJsonAsync<PrecioCriptoYa>(url);

                if (precio == null)
                    return StatusCode(500, $"Error al obtener precio de {saldo.CryptoCode}.");

                var valor = saldo.Cantidad * precio.TotalBid;

                resultado.Monedas.Add(new EstadoCriptoDto
                {
                    CryptoCode = saldo.CryptoCode,
                    Cantidad = saldo.Cantidad,
                    ValorEnPesos = Math.Round(valor, 2)
                });

                resultado.TotalARS += valor;
            }

            resultado.TotalARS = Math.Round(resultado.TotalARS, 2);
            return Ok(resultado);
        }





    }
}
