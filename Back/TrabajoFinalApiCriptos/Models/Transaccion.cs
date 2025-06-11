namespace TrabajoFinalApiCriptos.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int CriptomonedaId { get; set; }
        public CriptoMoneda Criptomoneda { get; set; }

        public int ExchangeId { get; set; } // ID del exchange utilizado
        public Exchange Exchange { get; set; } // Nombre del exchange utilizado

        public string TipoAccion { get; set; } // "purchase" o "sale"
        public decimal CantidadCripto { get; set; }
        public decimal MontoARS { get; set; } // ARS total gastado/recibido
        public DateTime FechaHora { get; set; }
    }
}
