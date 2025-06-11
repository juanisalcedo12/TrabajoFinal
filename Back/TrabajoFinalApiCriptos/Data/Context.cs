using TrabajoFinalApiCriptos.Models;
using Microsoft.EntityFrameworkCore;
namespace TrabajoFinalApiCriptos.Data   
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CriptoMoneda> CriptoMonedas { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
    }
}
