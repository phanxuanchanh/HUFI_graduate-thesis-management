using System;

namespace GraduateThesis.Common.Authorization
{
    public class AccountSession
    {
        public object AccountModel { get; set; }
        public string Role { get; set; }
        public DateTime LastSignInTime { get; set; }
    }
}
