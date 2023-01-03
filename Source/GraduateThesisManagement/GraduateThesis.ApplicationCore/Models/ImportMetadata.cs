using GraduateThesis.ApplicationCore.Enums;

#nullable disable

namespace GraduateThesis.ApplicationCore.Models;

public class ImportMetadata
{
    public int StartFromRow { get; set; }
    public ImportTypeOptions TypeOptions { get; set; }
    public string SheetName { get; set; }
}
