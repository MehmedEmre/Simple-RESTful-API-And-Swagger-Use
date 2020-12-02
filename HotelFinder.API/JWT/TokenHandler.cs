using HotelFinder.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.API.JWT
{
    public class TokenHandler
    {
        public IConfiguration Configuration { set; get; }

        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Token CreateAccessToken(User user)
        {
            Token token = new Token();

            var parameters = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Name)
            };

            //Security  Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hey. I am a secret Key. Sooo Who are you....."));
            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarlarını veriyoruz.
            token.Expiration = DateTime.Now.AddMinutes(5);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: "www.abc.com",
                audience: "www.cba.com",
                claims:parameters,//Tokenımızın içine gömdüğümüz ekstra bilgidir. Bu örnekte kullanıcı adı bilgisini tokenımıza verdik. İstersek bu bilgiyi daha sonra kullanabiliriz.
                expires:token.Expiration,//Token süresi beş dakika belirlendi
                notBefore:DateTime.Now,//Token üretildikten ne kadar sora devreye girsin?
                signingCredentials: signingCredentials
                );

            //Token oluşturucu sınıfından bir örnek oluşturuyoruz.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Token üretiyoruz.
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken(); ;

            return token;
        }

        public Guid CreateRefreshToken()
        {
            return Guid.NewGuid();
      
        }

    }
}
