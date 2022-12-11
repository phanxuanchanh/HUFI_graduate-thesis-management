using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Models
{
    public class SignInResultModel
    {
        public SignInStatus Status { get; set; }
        public string Message { get; set; }
    }
}
