using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Person> Persons  => Set<Person>();
        public DbSet<DaTable> Tables => Set<DaTable>();
        public DbSet<Booking> Bookings => Set<Booking>();

        IConfiguration _config;
        public IConfiguration Configuration { get { return _config; } }

        public ApplicationDbContext()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory).AddJsonFile
                        ("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        optional: true, reloadOnChange: true);

            _config = builder.Build();
        }
        public ApplicationDbContext(IConfiguration configuration) : base()
        {
            _config = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
