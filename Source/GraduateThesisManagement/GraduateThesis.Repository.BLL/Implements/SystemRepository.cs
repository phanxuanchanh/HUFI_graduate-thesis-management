using GraduateThesis.ApplicationCore.AppSettings;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraduateThesis.Repository.BLL.Implements;

public class SystemRepository : ISystemRepository
{
    private HufiGraduateThesisContext _context;

    internal SystemRepository(HufiGraduateThesisContext context)
    {
        _context = context;
    }

    public void Backup()
    {
        string backupFilePath = null;
        if (string.IsNullOrEmpty(AppDefaultValue.DbBackupFilePath))
            backupFilePath = $"{AppDefaultValue.DbName}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.bak";
        else
            backupFilePath = $"{AppDefaultValue.DbBackupFilePath}\\{AppDefaultValue.DbName}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.bak";
        
        DateTime expirateDate = DateTime.Now.AddDays(30);
        _context.Database.ExecuteSqlRaw(
            Resources.SqlQueryResource.BackupDb, 
            AppDefaultValue.DbName, 
            backupFilePath, 
            expirateDate
        );
    }

    public List<DbBackupHistoryOutput> GetDbBackupHistory()
    {
        //DateTime dateTime = DateTime.Now;
        //_context.Database.ExecuteSqlRaw("");

        return _context.DbBackupHistories
            .FromSqlRaw(Resources.SqlQueryResource.DbBackupHistory)
            .ToList().OrderByDescending(d => d.StartDate).ToList();
    }

    public void Restore()
    {
        throw new NotImplementedException();
    }
}
