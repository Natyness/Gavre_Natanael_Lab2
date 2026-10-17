using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natanael_Lab2.Data
{
    public class Gavre_Natanael_Lab2Context : DbContext
    {
        public Gavre_Natanael_Lab2Context (DbContextOptions<Gavre_Natanael_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Gavre_Natanael_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Gavre_Natanael_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<Gavre_Natanael_Lab2.Models.Author> Author { get; set; } = default!;
    }
}
