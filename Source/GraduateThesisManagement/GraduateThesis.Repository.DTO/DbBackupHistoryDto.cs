using System;

namespace GraduateThesis.Repository.DTO;

public class DbBackupHistoryOutput
{
    public string Server { get; set; }
    public string DatabaseName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string BackupType { get; set; }
    public decimal BackupSize { get; set; }
    public string LogicalDeviceName { get; set; }
    public string PhysicalDeviceName { get; set; }
    public string BackupSetName { get; set; }
    public string Description { get; set; }
}