using Microsoft.AspNetCore.Mvc;
using ExpenseApp.Models;
using ExpenseApp.Services;

namespace ExpenseApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Get all expenses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Expense>>> GetAllExpenses()
    {
        var expenses = await _expenseService.GetAllExpensesAsync();
        return Ok(expenses);
    }

    /// <summary>
    /// Get expenses by status
    /// </summary>
    /// <param name="status">Status name (Draft, Submitted, Approved, Rejected)</param>
    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<Expense>>> GetExpensesByStatus(string status)
    {
        var expenses = await _expenseService.GetExpensesByStatusAsync(status);
        return Ok(expenses);
    }

    /// <summary>
    /// Get expenses by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Expense>>> GetExpensesByUser(int userId)
    {
        var expenses = await _expenseService.GetExpensesByUserAsync(userId);
        return Ok(expenses);
    }

    /// <summary>
    /// Get expense by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetExpenseById(int id)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id);
        if (expense == null)
            return NotFound();
        return Ok(expense);
    }

    /// <summary>
    /// Create a new expense
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Expense>> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        // For demo purposes, using userId = 1
        var expense = await _expenseService.CreateExpenseAsync(request, 1);
        return CreatedAtAction(nameof(GetExpenseById), new { id = expense.ExpenseId }, expense);
    }

    /// <summary>
    /// Submit an expense for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<Expense>> SubmitExpense(int id)
    {
        try
        {
            var expense = await _expenseService.SubmitExpenseAsync(id);
            return Ok(expense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Approve an expense
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<ActionResult<Expense>> ApproveExpense(int id)
    {
        try
        {
            // For demo purposes, using reviewerId = 2 (Manager)
            var expense = await _expenseService.ApproveExpenseAsync(id, 2);
            return Ok(expense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Reject an expense
    /// </summary>
    [HttpPost("{id}/reject")]
    public async Task<ActionResult<Expense>> RejectExpense(int id)
    {
        try
        {
            // For demo purposes, using reviewerId = 2 (Manager)
            var expense = await _expenseService.RejectExpenseAsync(id, 2);
            return Ok(expense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get all expense categories
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<List<ExpenseCategory>>> GetCategories()
    {
        var categories = await _expenseService.GetCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Get all expense statuses
    /// </summary>
    [HttpGet("statuses")]
    public async Task<ActionResult<List<ExpenseStatus>>> GetStatuses()
    {
        var statuses = await _expenseService.GetStatusesAsync();
        return Ok(statuses);
    }
}
