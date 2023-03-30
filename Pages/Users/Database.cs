using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server_Dotnet.Pages.Users
{
    public class Database : DbContext
    {
        public DbSet<User> Users { get; set; }

        public string DbPath { get; } = "./Pages/Users/database.db";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class User
    {
        [Required]
        public string Id { get; set; }
		public bool Admin { get; set; } = false;

        public User(string Id)
        {
            this.Id = Id;
        }

		public User(string Id, bool Admin)
		{
			this.Id = Id;
			this.Admin = Admin;
		}
	}
}
