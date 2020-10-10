using System;
using System.Collections.Generic;
using System.Text;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AldeiaParental.Data
{
    public class ApplicationDbContext : IdentityDbContext<AldeiaParentalUser,AldeiaParentalRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AldeiaParental.Models.Region> Region { get; set; }
        public DbSet<AldeiaParental.Models.ServiceLocation> ServiceLocation { get; set; }
    }
}
