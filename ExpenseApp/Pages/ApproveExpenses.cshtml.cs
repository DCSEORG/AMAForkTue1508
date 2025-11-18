using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseApp.Models;
using ExpenseApp.Services;

namespace ExpenseApp.Pages;

public class ApproveExpensesModel : PageModel
{
    private readonly IExpenseService _expenseService;

    public ApproveExpensesModel(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public List<Expense> PendingExpenses { get; set; } = new();
    public string FilterText { get; set; } = "";

    public async Task OnGetAsync(string? filter)
    {
        FilterText = filter ?? "";
        
        var submitted = await _expenseService.GetExpensesByStatusAsync("Submitted");
        
        if (string.IsNullOrEmpty(filter))
        {
            PendingExpenses = submitted;
        }
        else
        {
            PendingExpenses = submitted
                .Where(e => 
                    e.CategoryName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    e.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }
    }

    public async Task<IActionResult> OnPostApproveAsync(int expenseId)
    {
        await _expenseService.ApproveExpenseAsync(expenseId, 2);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectAsync(int expenseId)
    {
        await _expenseService.RejectExpenseAsync(expenseId, 2);
        return RedirectToPage();
    }
}
