namespace TrabajoFinalApiCriptos.Models.Dtos_Views
{
    public class EstadoWalletDto
    {
        public int UsuarioId { get; set; }
        public decimal SaldoARS { get; set; }
        public List<EstadoCriptoDto> Monedas { get; set; }
        public decimal TotalARS { get; set; }
    }
}
