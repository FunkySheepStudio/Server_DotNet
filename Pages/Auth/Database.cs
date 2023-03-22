using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server_Dotnet.Pages.Auth
{
    public class Database : DbContext
    {
        public DbSet<Connection> Connections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase(databaseName : "authDb");
    }

    public class Connection
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string User { get; set; }

        public Connection(string Id, string User)
        {
            this.Id = Id;
            this.User = User;
        }
    }
}
