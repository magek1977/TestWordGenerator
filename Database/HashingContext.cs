using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWordGenerator.Database {
    class HashingContext : DbContext {
        public static String connectionString = "Server=.\\SQLExpress;Database=Hash;Trusted_Connection=True;";

        public DbSet<PasswordHash> PasswordHashes { get; set; }
        public DbSet<ProcessingInfo> ProcessingInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer(connectionString);
        }

    }
}
