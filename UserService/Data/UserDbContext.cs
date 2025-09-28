using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        //so base, ainda não tem nada
        //precisa configurar para o User
        //ApplicationDbContext do monolito FIAPCloudGamesProject
        //atualizado para UserDbContext no microservices
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais do modelo, se necessário
            // Não tinha no monolito, mas é bom garantir que o email seja único
            //validar como vai ficar no microservices
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
