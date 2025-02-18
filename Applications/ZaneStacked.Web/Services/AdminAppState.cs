using Microsoft.AspNetCore.Components;

namespace ZaneStacked.Web.Services;

public class AdminAppState
{
    public event Action? OnChange;
    private readonly BrowserService _browserService;
    private string _currentPageTitle = "Dashboard";
    private bool _isMenuOpen = false;

    public AdminAppState(BrowserService browserService)
    {
        _browserService = browserService;
    }

    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        set
        {
            if (_currentPageTitle != value)
            {
                _currentPageTitle = value;
                NotifyStateChanged();
            }
        }
    }

    public bool IsMenuOpen
    {
        get => _isMenuOpen;
        set
        {
            if (_isMenuOpen != value)
            {
                _isMenuOpen = value;
                NotifyStateChanged();
            }
        }
    }

    public async Task ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        if (!IsMenuOpen) return;

        if (await IsMobileViewAsync())
        {
            await Task.Delay(10); // Small delay to ensure UI updates
            NotifyStateChanged();
        }
    }

    public async Task NavigateTo(NavigationManager navManager, string route)
    {
        CurrentPageTitle = route switch
        {
            "/admin/dashboard" => "Dashboard",
            "/admin/skills" => "Skills",
            "/admin/projects" => "Projects",
            "/admin/settings" => "Settings",
            _ => "Admin"
        };

        navManager.NavigateTo(route);

        if (await IsMobileViewAsync())
            IsMenuOpen = false;
    }

    private async Task<bool> IsMobileViewAsync()
    {
        int width = await _browserService.GetWindowWidthAsync();
        return width < 768;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}