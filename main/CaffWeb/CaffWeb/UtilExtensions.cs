using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CaffWeb
{
    public static class UtilExtensions
    {
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("hu-hu");
        private const string dateFormatString = "yyyy. MMMM d. H:mm:ss";

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }

        public static string ToHufDateFormat(this DateTime dt)
        {
            return dt.ToString(dateFormatString, cultureInfo);
        }
    }
}
