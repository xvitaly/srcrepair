using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    public sealed class CFGTlx
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string[] SupportedGames { get; set; }

        public CFGTlx(string CfName, string CfFileName, string CfDescr, string[] CfGames)
        {
            //
        }
    }
}
