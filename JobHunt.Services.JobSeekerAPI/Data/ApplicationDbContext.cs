﻿using JobHunt.Services.JobSeekerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.JobSeekerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}