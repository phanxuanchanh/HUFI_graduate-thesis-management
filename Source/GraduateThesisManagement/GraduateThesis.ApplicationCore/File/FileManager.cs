using GraduateThesis.ApplicationCore.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.ObjectModel;

#nullable disable

namespace GraduateThesis.ApplicationCore.File;

public class FileManager : IFileManager
{
    private bool disposedValue;

    public FileManager()
    {

    }

    public bool IsValidType(IFormFile formFile, string contentType)
    {
        return (contentType == formFile.FileName);
    }

    public bool IsValidType(IFormFileCollection formFiles, string contentType)
    {
        return !formFiles.Any(f => f.ContentType != contentType);
    }



    public FileUploadStatus Upload(IFormFile formFile, string contentType, FileSaveOptions fileSaveOptions)
    {
        if(!IsValidType(formFile, contentType))
            return FileUploadStatus.InvalidType;

        if(fileSaveOptions == FileSaveOptions.GoogleCloud)
        {
            return FileUploadStatus.NotSupportedYet;
        }

        if (fileSaveOptions == FileSaveOptions.AmazonS3)
        {
            return FileUploadStatus.NotSupportedYet;
        }

        if (fileSaveOptions == FileSaveOptions.MicrosoftCloud)
        {
            return FileUploadStatus.NotSupportedYet;
        }

        SaveToLocal(formFile);
        return FileUploadStatus.Success;
    }

    public async Task<FileUploadStatus> UploadAsync(IFormFile formFile, FileSaveOptions fileSaveOptions)
    {
        return await Task.Run(() =>
        {
            return Upload(formFile, "", fileSaveOptions);
        });
    }

    private void SaveToLocal(IFormFile formFile)
    {
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream("", FileMode.Create);
            formFile.CopyTo(fileStream);
        }
        catch(Exception ex)
        {
            throw new Exception("An error occurred while saving the file!", ex);
        }
        finally
        {
            if (fileStream != null)
                fileStream.Dispose();
        }
    }

    private async Task SaveToLocalAsync(IFormFile formFile)
    {
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream("", FileMode.Create);
            await formFile.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving the file!", ex);
        }
        finally
        {
            if (fileStream != null)
                fileStream.Dispose();
        }
    }

    public FileStream Read(string path, params string[] contentTypes)
    {
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream(path, FileMode.Open);

            //bool checkContentType = contentTypes.Any(c => c == fileStream)

            return fileStream;
        }catch(Exception ex)
        {
            throw new Exception("", ex);
        }
        finally
        {
            if (fileStream != null)
                fileStream.Dispose();
        }
    }



    public TModel Read<TModel>()
    {
        return default(TModel);
    }

    public ICollection<TModel> ReadMany<TModel>()
    {
        return new Collection<TModel>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }
}
