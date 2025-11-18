using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseApp.Pages;

public class ChatModel : PageModel
{
    public bool ChatEnabled { get; set; }

    public void OnGet()
    {
        // Check if chat UI is enabled via configuration
        ChatEnabled = Configuration.GetValue<bool>("GenAI:EnableChatUI", false);
    }

    private IConfiguration Configuration { get; }

    public ChatModel(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}
