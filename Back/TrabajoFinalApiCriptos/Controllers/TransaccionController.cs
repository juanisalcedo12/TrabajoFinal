using Microsoft.AspNetCore.Mvc;
using TrabajoFinalApiCriptos.Data;
using TrabajoFinalApiCriptos.Models.Dtos_Views;
using TrabajoFinalApiCriptos.Models;
using Microsoft.EntityFrameworkCore;

namespace TrabajoFinalApiCriptos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : Controller
    {
        private readonly Context _context;
        private readonly HttpClient _http;

        public TransaccionController(Context context, HttpClient http)
        {
            _context = context;
            _http = http;
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaccion([FromBody] TransaccionDto dto)
        {
            // Validaciones básicas
            if (dto.CryptoAmount <= 0)
                return BadRequest("La cantidad debe ser mayor a 0.");

            if (dto.Action != "purchase" && dto.Action != "sale")
                return BadRequest("La acción debe ser 'purchase' o 'sale'.");

            // Obtener usuario
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return BadRequest("Usuario no encontrado.");

            // Obtener criptomoneda
            var cripto = await _context.CriptoMonedas
                .FirstOrDefaultAsync(c => c.Codigo.ToLower() == dto.CryptoCode.ToLower());

            if (cripto == null)
                return BadRequest("Criptomoneda no encontrada.");

            // Consultar precio actual desde CriptoYa
            var exchange = await _context.Exchanges.FindAsync(dto.ExchangeId);
            if (exchange == null)
                return BadRequest("Exchange no encontrado.");

            var url = $"https://criptoya.com/api/{exchange.Nombre.ToLower()}/{dto.CryptoCode.ToUpper()}/ars";

            var precio = await _http.GetFromJsonAsync<PrecioCriptoYa>(url);
            if (precio == null)
                return StatusCode(500, "Error al obtener precio de CriptoYa.");

            decimal precioUnitario = dto.Action == "purchase" ? precio.TotalAsk : precio.TotalBid;
            decimal montoTotal = dto.CryptoAmount * precioUnitario;

            if (dto.Action == "purchase")
            {
                // Validar saldo en ARS
                if (usuario.SaldoARS < montoTotal)
                    return BadRequest("Saldo insuficiente en pesos para comprar.");

                // Descontar el saldo
                usuario.SaldoARS -= montoTotal;
            }
            else if (dto.Action == "sale")
            {
                // Validar saldo en cripto
                var saldoCripto = _context.Transacciones
                    .Where(t => t.UsuarioId == dto.UsuarioId && t.CriptomonedaId == cripto.Id)
                    .AsEnumerable()
                    .Sum(t => t.TipoAccion == "purchase" ? t.CantidadCripto : -t.CantidadCripto);

                if (saldoCripto < dto.CryptoAmount)
                    return BadRequest("Saldo insuficiente de criptomonedas para vender.");

                // Sumar dinero al saldo en ARS
                usuario.SaldoARS += montoTotal;
            }

            // Crear transacción
            var transaccion = new Transaccion
            {
                UsuarioId = dto.UsuarioId,
                CriptomonedaId = cripto.Id,
                ExchangeId = exchange.Id,
                TipoAccion = dto.Action,
                CantidadCripto = dto.CryptoAmount,
                MontoARS = montoTotal,
                FechaHora = dto.FechaHora
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return Ok(new { transaccion.Id, transaccion.MontoARS });
        }

 
      

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionDetalleDto>>> GetTransacciones()
        {
            var transacciones = await _context.Transacciones
                .Include(t => t.Criptomoneda)
                .Include(t => t.Usuario)
                .Include(t => t.Exchange)
                .OrderByDescending(t => t.FechaHora)
                .Select(t => new TransaccionDetalleDto
                {
                    Id = t.Id,
                    Usuario = t.Usuario.Nombre,
                    CryptoCode = t.Criptomoneda.Codigo,
                    exchange = t.Exchange.Nombre,
                    Action = t.TipoAccion,
                    CryptoAmount = t.CantidadCripto,
                    MontoARS = t.MontoARS,
                    FechaHora = t.FechaHora
                })
                .ToListAsync();

            return Ok(transacciones);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TransaccionDetalleDto>> GetTransaccion(int id)
        {
            var transaccion = await _context.Transacciones
                .Include(t => t.Criptomoneda)
                .Include(t => t.Usuario)
                .Include(t => t.Exchange)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaccion == null)
                return NotFound();

            var dto = new TransaccionDetalleDto
            {
                Id = transaccion.Id,
                Usuario = transaccion.Usuario.Nombre,
                CryptoCode = transaccion.Criptomoneda.Codigo,
                exchange = transaccion.Exchange.Nombre,
                Action = transaccion.TipoAccion,
                CryptoAmount = transaccion.CantidadCripto,
                MontoARS = transaccion.MontoARS,
                FechaHora = transaccion.FechaHora
            };

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
                return NotFound("Transacción no encontrada.");

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return Ok("Transacción eliminada.");
        }

    }
}
