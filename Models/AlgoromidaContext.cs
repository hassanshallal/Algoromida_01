using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Algoromida_01.Models
{
    public class AlgoromidaContext : IdentityDbContext<AlgoromidaUser>
    {
        public AlgoromidaContext(DbContextOptions<AlgoromidaContext> options)
            : base(options)
        {
        }

        public DbSet<UserBotInteraction> Interactions { get; set; }
	public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
