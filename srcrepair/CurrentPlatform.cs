using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair
{
    public class CurrentPlatform
    {
        public static int RunningOS
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Unix:
                        return (Directory.Exists("/Applications") & Directory.Exists("/System") & Directory.Exists("/Users") & Directory.Exists("/Volumes")) ? 1 : 2;
                    case PlatformID.MacOSX:
                        return 1;
                    default: return 0;
                }
            }
        }
    }
}
