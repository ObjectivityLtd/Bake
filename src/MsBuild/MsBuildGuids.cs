using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.CD.MsBuild
{
    public static class MsBuildGuids
    {
        public static readonly string CSharp = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        public static readonly string MVC4 = "{E3E379DF-F4C6-4180-9B81-6769533ABE47}";

        public static readonly string MVC5 = "{349C5851-65DF-11DA-9384-00065B846F21}";

        public static readonly string WebSite = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

        public static bool IsSupportedSlnTypeIdentifier(string guid)
        {
            return Equals(guid, CSharp) || Equals(guid, WebSite);
        }

        public static bool IsWebApplication(string guid)
        {
            return Equals(guid, MVC4) || Equals(guid, MVC5);
        }

        public static bool IsWebSite(string guid)
        {
            return Equals(guid, WebSite);
        }

        private static bool Equals(string guid1, string guid2)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(guid1, guid2);
        }

    }
}