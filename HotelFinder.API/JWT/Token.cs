using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinder.API.JWT
{
    /// <summary>
    /// 
    /// </summary>

    public class Token
    {
        public string AccessToken { set;get;}
        public DateTime Expiration { set; get; }
        public Guid RefreshToken { set; get; }

    }
}
