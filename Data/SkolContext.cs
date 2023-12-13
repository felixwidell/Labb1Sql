using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb1SQL___.Models;

namespace Labb1Sql___.Data
{
    internal class SkolContext : DbContext
    {
        public DbSet<Betyg> Betyg { get; set; }
        public DbSet<Elever> Elever { get; set; }
        public DbSet<Klasser> Klasser { get; set; }
        public DbSet<Kurser> Kurser { get; set; }
        public DbSet<Personal> Personal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Skola;Integrated Security=True");
            }
        }
    }
}
