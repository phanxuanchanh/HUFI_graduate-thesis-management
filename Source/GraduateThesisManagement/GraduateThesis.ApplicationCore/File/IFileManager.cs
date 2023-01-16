
namespace GraduateThesis.ApplicationCore.File;

public interface IFileManager: IDisposable
{
    void Save();
    void Delete();
}
