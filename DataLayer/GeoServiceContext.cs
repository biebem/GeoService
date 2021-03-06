﻿using DataLayer.BaseClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class GeoServiceContext  :  DbContext
    {
        /// <summary>
        /// DbSet that contains continents from database
        /// </summary>
        public DbSet<DContinent> Continents { get; set; }
        /// <summary>
        /// DbSet that contains rivers from database
        /// </summary>
        public DbSet<DRiver> Rivers { get; set; }
        /// <summary>
        /// connectionstring for DB
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public GeoServiceContext() { }
        /// <summary>
        /// Constructor that sets connectionstring
        /// </summary>
        /// <param name="db"></param>
        public GeoServiceContext(string db = "MainDB") : base()
        {
            SetConnectionString(db);
        }
        /// <summary>
        /// Sets the connectionstring derived from the appsettings.jsonfile
        /// </summary>
        /// <param name="db">what Database to use connectionstring from </param>
        private void SetConnectionString(string db = "MainDB") 
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("dataappsettings.json", optional: false);
            var configuration = builder.Build();
            switch (db)
            {
                case "MainDB":
                    connectionString = configuration.GetConnectionString("GeoServiceConnection").ToString();
                    break;
                case "TestDB":
                    connectionString = configuration.GetConnectionString("GeoServiceTestConnection").ToString();
                    break;
            }
        }

        /// <summary>
        /// Sets connectionString on the configure of the app
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(connectionString))
                SetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
        }
        /// <summary>
        /// Used to configure the relationship between river and country
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryRiver>()
                 .HasKey(cr => new { cr.CountryKey, cr.RiverKey });
            modelBuilder.Entity<CountryRiver>()
                .HasOne(cr => cr.Country)
                .WithMany(c => c.Rivers)
                .HasForeignKey(cr => cr.CountryKey);
            modelBuilder.Entity<CountryRiver>()
                .HasOne(cr => cr.River)
                .WithMany(r => r.Countries)
                .HasForeignKey(cr => cr.RiverKey);
        }
    }
}
