namespace GraduateThesis.Repository.DTO;

public class StudentGroupDtOutput
{
    public string StudentId { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FullName { get { return $"{Surname.Trim(' ')} {Name.Trim(' ')}"; } }
    public string Email { get; set; }
    public bool IsLeader { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }
    public decimal? SupvPoint { get; set; }
    public decimal? CtrArgPoint { get; set; }
    public decimal? CmtePoint { get; set; }
}
