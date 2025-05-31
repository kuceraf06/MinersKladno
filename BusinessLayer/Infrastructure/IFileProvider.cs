namespace Miners.Web.BusinessLayer.Infrastructure;

public interface IFileProvider
{
    string SaveFile(string filename, byte[] data);
    //FileResult GetFile(string id);
    string SaveFile(Guid guid, string filename, byte[] data);
    void Delete(string fileName);
}