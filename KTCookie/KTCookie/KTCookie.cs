using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KTCookie
{
    public class KTCookie
    {
        /*
         * If there is no cookie has already created, firstly new cookie creating with name
         */
        public static bool SetCookie(string cookieName, List<KTCookieObject> parameters, DateTime expireDate)
        {
            var culture = CultureInfo.CurrentCulture;
            var dtExpire = expireDate.ToString(culture);

            try
            {
                var liParameters = new List<KTCookieObject>();
                foreach (var param in parameters)
                {
                    var newParameter = new KTCookieObject
                    {
                        Name = param.Name,
                        Data = param.Data
                    };
                    liParameters.Add(newParameter);
                }

                var httpCookie = HttpContext.Current.Response.Cookies[cookieName];
                var expire = DateTime.Now.AddDays(1);
                if (httpCookie != null)
                {
                    foreach (var parameter in liParameters)
                    {
                        httpCookie[parameter.Name] = parameter.Data;
                    }

                    DateTime.TryParse(dtExpire, culture, DateTimeStyles.AssumeLocal, out expire);

                    httpCookie["expire"] = dtExpire;
                    httpCookie.Expires = expire;
                    httpCookie.HttpOnly = true;
                }
                else
                {
                    var aCookie = new HttpCookie(cookieName);

                    foreach (var parameter in liParameters)
                    {
                        aCookie.Values[parameter.Name] = parameter.Data;
                    }
                    aCookie.Values["expire"] = dtExpire;
                    aCookie.Expires = expire;
                    aCookie.HttpOnly = true;

                    HttpContext.Current.Response.Cookies.Add(aCookie);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /*
         * SetCookie override with constant name
         */
        public static bool SetCookie(List<KTCookieObject> parameters, DateTime expireDate)
        {
            const string cookieName = Constant.CookieName;

            return SetCookie(cookieName, parameters, expireDate);
        }

        /*
         * Gets data from cookie as 
         */
        public static IEnumerable<KTCookieObject> GetCookie(string cookieName, string password)
        {
            var liReturn = new List<KTCookieObject>();
            try
            {
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    var userInfoCookieCollection = cookie.Values;
                    var strExpire = userInfoCookieCollection["expire"];


                    DateTime expireDate;
                    DateTime.TryParse(strExpire, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out expireDate);

                    if (expireDate > DateTime.Now.AddHours(1))
                    {
                        liReturn.AddRange(from object par in userInfoCookieCollection
                            let data = HttpContext.Current.Server.HtmlEncode(userInfoCookieCollection[par.ToString()])
                            select new KTCookieObject
                            {
                                Name = par.ToString(),
                                Data = data
                            });
                    }
                }
            }
            catch
            {

            }
            return liReturn;
        }

        /*
         * To set cookie expired
         */
        public static bool RemoveCookie(string cookieName)
        {
            try
            {
                var aCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
