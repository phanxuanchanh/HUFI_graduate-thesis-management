using Microsoft.AspNetCore.Http;

namespace GraduateThesis.Repository.DTO;

public class ThesisSubmissionInput
{
    public string ThesisId { get; set; }
    public string GroupId { get; set; }
    public IFormFile File { get; set; }
}
