namespace TrabajoFinalApiCriptos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }  // o Email como identificador único
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public decimal SaldoARS { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
