using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWithIA.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWithIA.Data;
public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
