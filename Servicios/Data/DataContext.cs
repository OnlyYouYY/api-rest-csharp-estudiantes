using Microsoft.EntityFrameworkCore;
using Servicios.Models;

namespace Servicios.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

            
        }

        public DbSet<Estudiante> Estudiantes {get; set;}
    }
}