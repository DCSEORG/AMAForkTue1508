using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseApp.Models;
using ExpenseApp.Services;

namespace ExpenseApp.Pages;

public class IndexModel : PageModel
{
    private readonly IExpenseService _expenseService;

    public IndexModel(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public List<Expense> Expenses { get; set; } = new();
    public string FilterText { get; set; } = "";

    public async Task OnGetAsync(string? filter)
    {
        FilterText = filter ?? "";
        
        if (string.IsNullOrEmpty(filter))
        {
            Expenses = await _expenseService.GetAllExpensesAsync();
        }
        else
        {
            Expenses = (await _expenseService.GetAllExpensesAsync())
                .Where(e => 
                    e.CategoryName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    e.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true ||
                    e.StatusName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
