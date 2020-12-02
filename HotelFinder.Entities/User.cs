using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HotelFinder.Entities
{
    public class User
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Username { set; get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RefreshToken { set; get; }
        //Kullanıcı için üretilmiş olan refresh token değerini tutacak olan kolondur.
        public DateTime? RefreshTokenEndDate { set; get; }
        //Üretilen refresh token değerinin işlev/kullanım süresini belirleyecek olan zaman bilgisini tutan alandır.

        //Not: Her zaman Refresh Token Expiration, Access Token Expiration’dan fazla olmalıdır!
    }
}
