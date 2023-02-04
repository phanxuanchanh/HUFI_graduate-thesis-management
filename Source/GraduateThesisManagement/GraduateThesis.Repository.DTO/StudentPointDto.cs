
namespace GraduateThesis.Repository.DTO;

public class StudentPointInput
{
    public string StudentId { get; set; }

    public decimal? SupervisorPoint { get; set; }
    public decimal? CriticialPoint { get; set; }
    public decimal? CommitteePoint { get; set; }
}

public class StudentPointOutput : StudentPointInput
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FullName { get { return $"{Surname.Trim(' ')} {Name.Trim(' ')}"; } }
}