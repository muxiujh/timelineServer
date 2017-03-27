using System.Collections.Generic;
using WxhnecServer;

namespace JCore
{
    public class G
    {
        public static Dictionary<string, dynamic> Config = (new ConfigModel()).Cache();
    }
}