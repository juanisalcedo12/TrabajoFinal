namespace TrabajoFinalApiCriptos.Models.Dtos_Views
{
    public class UsuarioCreateDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public decimal SaldoARS { get; set; } = 0;

    }
}
