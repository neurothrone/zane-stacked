using Microsoft.JSInterop;

namespace ZaneStacked.Web.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetThemeAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "theme") ?? "dark";
    }

    public async Task SetThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme);
        await _jsRuntime.InvokeVoidAsync("setTheme", theme);
    }
}