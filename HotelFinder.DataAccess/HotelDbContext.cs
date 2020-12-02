using HotelFinder.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinder.DataAccess
{
    public class HotelDbContext:DbContext
    {

        public DbSet<Hotel> HotelTable { set; get; }
        public DbSet<User> UserTable { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-D6E78MU\\SQLEXPRESS; database=HotelDb ;integrated security=true;");
        }
    }
}
