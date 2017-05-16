using System;

namespace Bake.API.MsBuild
{
    public static class MsBuildGuids
    {
        public static readonly string CSharp = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        public static readonly string CSharpCore = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";

        public static readonly string MVC4 = "{E3E379DF-F4C6-4180-9B81-6769533ABE47}";

        public static readonly string MVC5 = "{349C5851-65DF-11DA-9384-00065B846F21}";

        public static readonly string WebSite = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

        public static readonly string Test = "{3AC096D0-A1C2-E12C-1390-A8335801FDAB}";

        public static bool IsSupportedSlnTypeIdentifier(string guid)
        {
            return IsCSharp(guid) || IsWebSite(guid);
        }

        public static bool IsCSharp(string guid)
        {
            return Equals(guid, CSharp) || Equals(guid, CSharpCore);
        }

        public static bool IsWebApplication(string guid)
        {
            return Equals(guid, MVC4) || Equals(guid, MVC5);
        }

        public static bool IsWebSite(string guid)
        {
            return Equals(guid, WebSite);
        }

        public static bool IsTest(string guid)
        {
            return Equals(guid, Test);
        }

        private static bool Equals(string guid1, string guid2)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(guid1, guid2);
        }

    }
}