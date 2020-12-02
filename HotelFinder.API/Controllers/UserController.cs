using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelFinder.API.JWT;
using HotelFinder.Buisness.Abstract;
using HotelFinder.Buisness.Concrete;
using HotelFinder.Entities;
using HotelFinder.Entities.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HotelFinder.API.Controllers
{
    //[Authorize] // “Authorize” attribute’u ile işaretlenerek yetkisiz erişim engellenir bir hale getirilmiştir.
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;
        private IConfiguration Configuration { set; get; }

        public UserController(IUserService userService,IConfiguration _Configuration)
        {
            _userService = userService;
            Configuration = _Configuration;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<Boolean> CreateUser([FromBody]User userModel)
        {
            if(userModel != null)
            {
                return await _userService.CreateUser(userModel);
            }

            return false;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<Token> Login([FromBody]UserViewModel userModel)
        {
            List<User> userList = await _userService.GetAllUsers();

            User user = userList.Find(x => x.Username == userModel.Username && x.Password == userModel.Password);

            if(user != null)
            {
                //Token üretiliyor
                TokenHandler tokenHandler = new TokenHandler(Configuration);

                Token token = tokenHandler.CreateAccessToken(user);

                //Refresh token Users tablosuna işleniyor
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                //Her zaman Refresh Token Expiration, Access Token Expiration’dan fazla olmalıdır!

                 _userService.UpdateUser(user);

                return token;
            }

            return null;

        }

        [HttpGet]
        [Route("[action]/{refreshToken}")]
        public async Task<Token> RefreshTokenLogin(string refreshToken)
        {
            List<User> userList = await _userService.GetAllUsers();

            User user = userList.Find(x=>x.RefreshToken == Guid.Parse(refreshToken));

            if (user != null && user?.RefreshTokenEndDate > DateTime.Now)
            {
                TokenHandler tokenHandler = new TokenHandler(Configuration);

                Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

                return token;
            }

            return null;
        }






    }
}
