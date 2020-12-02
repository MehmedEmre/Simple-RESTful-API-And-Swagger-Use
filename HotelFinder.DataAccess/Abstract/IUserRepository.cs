using HotelFinder.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.DataAccess.Abstract
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<Boolean> CreateUser(User user);

        void UpdateUser(User user);
    }
}
