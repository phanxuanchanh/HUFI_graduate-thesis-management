using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Interfaces
{
    public  interface ICouncilRepository : IAsyncSubRepository<CouncilInput, CouncilOutput, string>, IAsyncAccountPattern
    {
    }
}
