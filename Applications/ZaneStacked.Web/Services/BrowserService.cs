using Microsoft.JSInterop;

namespace ZaneStacked.Web.Services;

public class BrowserService
{
    private readonly IJSRuntime _jsRuntime;

    public BrowserService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<int> GetWindowWidthAsync()
    {
        return await _jsRuntime.InvokeAsync<int>("getWindowWidth");
    }
}