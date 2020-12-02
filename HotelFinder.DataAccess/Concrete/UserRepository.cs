using HotelFinder.DataAccess.Abstract;
using HotelFinder.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.DataAccess.Concrete
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> CreateUser(User user)
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                context.UserTable.Add(user);
                var val = await context.SaveChangesAsync();

                if(val > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

               
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            using(HotelDbContext context = new HotelDbContext())
            {
                return await context.UserTable.ToListAsync();
            }
        }

        public  void UpdateUser(User user)
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                context.Update(user);
                context.SaveChanges();
            }
        }
    }
}
