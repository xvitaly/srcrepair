using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair.core
{
    public class Type4Video : Type1Video
    {
        public Type4Video(string SAppName, bool ReadNow = true) : base (SAppName, false)
        {
            if (ReadNow)
            {
                ReadSettings();
            }
        }
    }
}
