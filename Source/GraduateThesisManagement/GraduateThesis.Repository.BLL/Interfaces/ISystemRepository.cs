
using GraduateThesis.Repository.DTO;
using System.Collections.Generic;

namespace GraduateThesis.Repository.BLL.Interfaces;

public interface ISystemRepository
{
    void Backup();
    List<DbBackupHistoryOutput> GetDbBackupHistory();
    void Restore();
}
