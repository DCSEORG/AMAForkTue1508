using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseApp.Models;
using ExpenseApp.Services;

namespace ExpenseApp.Pages;

public class AddExpenseModel : PageModel
{
    private readonly IExpenseService _expenseService;

    public AddExpenseModel(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [BindProperty]
    public decimal Amount { get; set; }

    [BindProperty]
    public DateTime ExpenseDate { get; set; } = DateTime.Today;

    [BindProperty]
    public int CategoryId { get; set; }

    [BindProperty]
    public string? Description { get; set; }

    public List<ExpenseCategory> Categories { get; set; } = new();
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await _expenseService.GetCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Categories = await _expenseService.GetCategoriesAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new CreateExpenseRequest
        {
            Amount = Amount,
            ExpenseDate = ExpenseDate,
            CategoryId = CategoryId,
            Description = Description
        };

        var expense = await _expenseService.CreateExpenseAsync(request, 1);
        
        // Auto-submit the expense
        await _expenseService.SubmitExpenseAsync(expense.ExpenseId);

        return RedirectToPage("/Index");
    }
}
