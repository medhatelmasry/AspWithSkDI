using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AspWithSkDI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Kernel _kernel;

    [BindProperty]
    public string? Reply { get; set; }

    public IndexModel(ILogger<IndexModel> logger, Kernel kernel)
    {
        _logger = logger;
        _kernel = kernel;
    }

    public void OnGet()
    {

    }

    // action method that receives user prompt from the form
    public async Task<IActionResult> OnPostAsync(string userPrompt)
    {
        // get a chat completion service
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        // Create a new chat by specifying the assistant
        ChatHistory chat = new(@"
        You are an AI assistant that helps people find information about baking. 
        The baked item must be easy, tasty, and cheap. 
        I don't want to spend more than $10 on ingredients.
        I don't want to spend more than 30 minutes preparing.
        I don't want to spend more than 30 minutes baking."
        );

        chat.AddUserMessage(userPrompt);

        var response = await chatCompletionService.GetChatMessageContentAsync(chat, kernel: _kernel);

        Reply = response.Content!.Replace("\n", "<br>");

        return Page();
    }
}
