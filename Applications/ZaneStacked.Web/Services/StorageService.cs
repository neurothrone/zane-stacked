namespace ZaneStacked.Web.Services;

public class StorageService
{
    public string BaseUrl { get; }

    public StorageService(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
}