namespace TrabajoFinalApiCriptos.Data
{
    public class TransaccionDetalleDto
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string CryptoCode { get; set; }
        public string Action { get; set; }
        public decimal CryptoAmount { get; set; }
        public decimal MontoARS { get; set; }
        public string exchange { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
