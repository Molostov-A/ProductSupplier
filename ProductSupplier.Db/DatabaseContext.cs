﻿using Microsoft.EntityFrameworkCore;
using ProductSupplier.Models;

namespace ProductSupplier.Db
{
    public sealed class DatabaseContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }
    }
}
