using Microsoft.JSInterop;

namespace ZaneStacked.Web.Services;

public class ToastService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Random _random = new();

    public ToastService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowToastAsync(ToastType type)
    {
        var (title, message, color) = GetRandomToastContent(type);
        await _jsRuntime.InvokeVoidAsync("showBootstrapToast", title, message, color);
    }

    private (string Title, string Message, string Color) GetRandomToastContent(ToastType type)
    {
        var titles = new Dictionary<ToastType, List<string>>
        {
            {
                ToastType.Success,
                [
                    "Success!",
                    "You Did It!",
                    "Against All Odds...",
                    "Victory is Yours!",
                    "Barely Made It!"
                ]
            },
            {
                ToastType.Warning,
                [
                    "Uh-oh...",
                    "You Done It Now",
                    "Not Ideal...",
                    "Proceed with Caution",
                    "Reconsider Your Choices"
                ]
            },
            {
                ToastType.Error,
                [
                    "Catastrophic Failure",
                    "Reality Has Collapsed",
                    "This Was Avoidable",
                    "The End is Near",
                    "It's All Gone Wrong"
                ]
            }
        };

        var messages = new Dictionary<ToastType, List<string>>
        {
            {
                ToastType.Success,
                [
                    "Somehow, it worked.",
                    "The universe smiles upon you.",
                    "That was not supposed to happen, but okay.",
                    "You cheated death... this time.",
                    "No one thought it would work, yet here we are."
                ]
            },

            {
                ToastType.Warning,
                [
                    "Do you have a backup plan?",
                    "Congratulations, you've voided the warranty.",
                    "Hope you weren't attached to that data.",
                    "This might come back to haunt you.",
                    "Your future self is screaming right now."
                ]
            },

            {
                ToastType.Error,
                [
                    "Rollback failed. Panic accordingly.",
                    "All known logic has been defied.",
                    "The system is now self-aware. Good luck.",
                    "This is the part where we pretend nothing happened.",
                    "Have you tried turning reality off and on again?"
                ]
            }
        };

        var colors = new Dictionary<ToastType, string>
        {
            { ToastType.Success, "text-success" },
            { ToastType.Warning, "text-warning" },
            { ToastType.Error, "text-danger" }
        };

        string randomTitle = titles[type][_random.Next(titles[type].Count)];
        string randomMessage = messages[type][_random.Next(messages[type].Count)];
        string color = colors[type];

        return (randomTitle, randomMessage, color);
    }
}

public enum ToastType
{
    Success,
    Warning,
    Error
}