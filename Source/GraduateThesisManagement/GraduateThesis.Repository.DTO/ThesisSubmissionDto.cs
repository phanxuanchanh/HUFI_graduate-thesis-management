using Microsoft.AspNetCore.Http;

namespace GraduateThesis.Repository.DTO;

public class ThesisSubmissionInput
{
    public string ThesisId { get; set; }
    public string GroupId { get; set; }
    public IFormFile DocumentFile { get; set; }
    public IFormFile PresentationFile { get; set; }
    public IFormFile PdfFile { get; set; }
    public IFormFile SourceCode { get; set; }
}
