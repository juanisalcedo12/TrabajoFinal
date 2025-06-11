namespace TrabajoFinalApiCriptos.Models
{
    public class CriptoMoneda
    {

        public int Id { get; set; }
        public string Codigo { get; set; } // Ej: "btc", "usdc"
        public string Nombre { get; set; } // Ej: "Bitcoin"

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
