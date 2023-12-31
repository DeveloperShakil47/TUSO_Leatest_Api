using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUSO.Utilities.Constants
{
    public static class ExtensionMethod
    {
        public static string ToFormat24h(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd, HH:mm:ss");
        }
    }
}
