using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    public sealed class SteamConv
    {
        public static int GetUserID(string Sid32)
        {
            string[] SidArr = Sid32.Split(':');
            return Convert.ToInt32(SidArr[2]) * 2 + Convert.ToInt32(SidArr[1]);
        }
        
        /// <summary>
        /// Преобразовывает старый формат SteamID32 в новый SteamIDv3.
        /// </summary>
        /// <param name="Sid32">SteamID32</param>
        /// <returns>SteamIDv3</returns>
        public static string ConvSid32Sidv3(string Sid32)
        {
            return String.Format("[U:1:{0}]", GetUserID(Sid32));
        }
    }
}
