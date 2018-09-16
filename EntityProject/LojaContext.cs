using Microsoft.EntityFrameworkCore;
using System;

namespace EntityProject
{
    public class LojaContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        private DbSet<Compra> Compras { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LojaDB;Trusted_Connection=true;");
        }
    }
}