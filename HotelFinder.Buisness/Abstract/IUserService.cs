using HotelFinder.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.Buisness.Abstract
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<Boolean> CreateUser(User user);
        void UpdateUser(User user);
    }
}
