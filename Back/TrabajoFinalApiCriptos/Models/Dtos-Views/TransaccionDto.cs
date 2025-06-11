namespace TrabajoFinalApiCriptos.Data
{
    public class TransaccionDto
    {
        public int UsuarioId { get; set; } // opcional si usás auth
        public string CryptoCode { get; set; }  // Ej: "btc", "usdc"
        public string Action { get; set; }      // "purchase" o "sale"
        public int ExchangeId { get; set; } // opcional, si usás exchange específico
        public decimal CryptoAmount { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
