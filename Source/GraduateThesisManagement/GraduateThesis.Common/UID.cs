using System;

namespace GraduateThesis.Common
{
    public class UID
    {
        public static string GetShortUID()
        {
            return ShortUID.ShortUID.Generate();
        }

        public static string GetUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
