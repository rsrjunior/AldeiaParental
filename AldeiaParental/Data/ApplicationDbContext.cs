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
        public DbSet<Region> Region { get; set; }
        public DbSet<ServiceLocation> ServiceLocation { get; set; }
        public DbSet<AldeiaParental.Models.PersonalDocument> PersonalDocument { get; set; }
        public DbSet<AldeiaParental.Models.Service> Service { get; set; }
    }
}
