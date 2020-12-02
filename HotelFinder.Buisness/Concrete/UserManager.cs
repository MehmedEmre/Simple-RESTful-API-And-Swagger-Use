using HotelFinder.Buisness.Abstract;
using HotelFinder.DataAccess.Abstract;
using HotelFinder.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinder.Buisness.Concrete
{
    public class UserManager:IUserService
    {
        private IUserRepository _userRepository { set; get; }

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async  Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<bool> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }

        public  void UpdateUser(User user)
        {
             _userRepository.UpdateUser(user);
        }
    }
}
