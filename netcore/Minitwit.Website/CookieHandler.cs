using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace Minitwit.Website
{
    public class CookieHandler
    {
        public CookieOptions SetCookie(int? expireTime)
        {
            CookieOptions options = new CookieOptions();

            if (expireTime.HasValue)
                options.Expires = DateTime.UtcNow.AddMinutes(expireTime.Value);
            else
                options.Expires = DateTime.UtcNow.AddMilliseconds(100);

            return options;
        }

        public static bool LoggedIn(HttpRequest req)
        {
            var cookie = req.Cookies["user"];
            if (cookie == null) return false;
            return true;
        }

    }
}
