# KTCookie
==========

Creating generic cookies and getting info as an IENumarable list

Usage
----

### Import

Add project into your solution and then add to your references. After adding references add

> using KTCookie;

to your class references, which you want to use.


### SetCookie

Import parameters cookieName (optional), cookie parameters and expire date.

> var cookieSaved = KTCookie.SetCookie("[COOKIENAME]", new List<CookieObject>{
>						new CookieObject { Name = "[NAME]", Data = "[DATA]" }
>					}, [Expire date as Datetime]);


### GetCookie

Returns CookieObject list.

> var cookieData = KTCookie.GetCookie("[COOKIENAME]");