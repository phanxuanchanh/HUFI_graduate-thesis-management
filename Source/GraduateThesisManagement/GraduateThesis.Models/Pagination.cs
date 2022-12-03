using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Models
{
    public class Pagination<T_DTO>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public List<T_DTO> Items { get; set; } = default!;
    }
}
