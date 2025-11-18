using ExpenseApp.Models;

namespace ExpenseApp.Services;

public interface IExpenseService
{
    Task<List<Expense>> GetAllExpensesAsync();
    Task<List<Expense>> GetExpensesByStatusAsync(string status);
    Task<List<Expense>> GetExpensesByUserAsync(int userId);
    Task<Expense?> GetExpenseByIdAsync(int expenseId);
    Task<Expense> CreateExpenseAsync(CreateExpenseRequest request, int userId);
    Task<Expense> SubmitExpenseAsync(int expenseId);
    Task<Expense> ApproveExpenseAsync(int expenseId, int reviewerId);
    Task<Expense> RejectExpenseAsync(int expenseId, int reviewerId);
    Task<List<ExpenseCategory>> GetCategoriesAsync();
    Task<List<ExpenseStatus>> GetStatusesAsync();
}

public class DummyExpenseService : IExpenseService
{
    private readonly List<Expense> _expenses;
    private readonly List<ExpenseCategory> _categories;
    private readonly List<ExpenseStatus> _statuses;
    private int _nextExpenseId = 5;

    public DummyExpenseService()
    {
        // Initialize categories
        _categories = new List<ExpenseCategory>
        {
            new ExpenseCategory { CategoryId = 1, CategoryName = "Travel", IsActive = true },
            new ExpenseCategory { CategoryId = 2, CategoryName = "Meals", IsActive = true },
            new ExpenseCategory { CategoryId = 3, CategoryName = "Supplies", IsActive = true },
            new ExpenseCategory { CategoryId = 4, CategoryName = "Accommodation", IsActive = true },
            new ExpenseCategory { CategoryId = 5, CategoryName = "Other", IsActive = true }
        };

        // Initialize statuses
        _statuses = new List<ExpenseStatus>
        {
            new ExpenseStatus { StatusId = 1, StatusName = "Draft" },
            new ExpenseStatus { StatusId = 2, StatusName = "Submitted" },
            new ExpenseStatus { StatusId = 3, StatusName = "Approved" },
            new ExpenseStatus { StatusId = 4, StatusName = "Rejected" }
        };

        // Initialize dummy expenses with dates avoiding month/day confusion
        _expenses = new List<Expense>
        {
            new Expense
            {
                ExpenseId = 1,
                UserId = 1,
                UserName = "Alice Example",
                CategoryId = 1,
                CategoryName = "Travel",
                StatusId = 2,
                StatusName = "Submitted",
                AmountMinor = 12000, // £120.00
                Currency = "GBP",
                ExpenseDate = DateTime.Now.AddDays(-10),
                Description = "Taxi from airport to client site",
                ReceiptFile = "/receipts/taxi.jpg",
                SubmittedAt = DateTime.Now.AddDays(-9),
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new Expense
            {
                ExpenseId = 2,
                UserId = 1,
                UserName = "Alice Example",
                CategoryId = 2,
                CategoryName = "Meals",
                StatusId = 2,
                StatusName = "Submitted",
                AmountMinor = 6900, // £69.00
                Currency = "GBP",
                ExpenseDate = DateTime.Now.AddDays(-20),
                Description = "Client lunch meeting",
                ReceiptFile = "/receipts/lunch.jpg",
                SubmittedAt = DateTime.Now.AddDays(-19),
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new Expense
            {
                ExpenseId = 3,
                UserId = 1,
                UserName = "Alice Example",
                CategoryId = 3,
                CategoryName = "Office Supplies",
                StatusId = 3,
                StatusName = "Approved",
                AmountMinor = 9950, // £99.50
                Currency = "GBP",
                ExpenseDate = DateTime.Now.AddDays(-30),
                Description = "Office stationery and supplies",
                ReceiptFile = "/receipts/supplies.jpg",
                SubmittedAt = DateTime.Now.AddDays(-29),
                ReviewedBy = 2,
                ReviewedByName = "Bob Manager",
                ReviewedAt = DateTime.Now.AddDays(-28),
                CreatedAt = DateTime.Now.AddDays(-30)
            },
            new Expense
            {
                ExpenseId = 4,
                UserId = 1,
                UserName = "Alice Example",
                CategoryId = 1,
                CategoryName = "Transport",
                StatusId = 3,
                StatusName = "Approved",
                AmountMinor = 1920, // £19.20
                Currency = "GBP",
                ExpenseDate = DateTime.Now.AddDays(-40),
                Description = "Train tickets for conference",
                ReceiptFile = "/receipts/train.jpg",
                SubmittedAt = DateTime.Now.AddDays(-39),
                ReviewedBy = 2,
                ReviewedByName = "Bob Manager",
                ReviewedAt = DateTime.Now.AddDays(-38),
                CreatedAt = DateTime.Now.AddDays(-40)
            }
        };
    }

    public Task<List<Expense>> GetAllExpensesAsync()
    {
        return Task.FromResult(_expenses.OrderByDescending(e => e.CreatedAt).ToList());
    }

    public Task<List<Expense>> GetExpensesByStatusAsync(string status)
    {
        var filtered = _expenses
            .Where(e => e.StatusName.Equals(status, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.CreatedAt)
            .ToList();
        return Task.FromResult(filtered);
    }

    public Task<List<Expense>> GetExpensesByUserAsync(int userId)
    {
        var filtered = _expenses
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.CreatedAt)
            .ToList();
        return Task.FromResult(filtered);
    }

    public Task<Expense?> GetExpenseByIdAsync(int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        return Task.FromResult(expense);
    }

    public Task<Expense> CreateExpenseAsync(CreateExpenseRequest request, int userId)
    {
        var category = _categories.FirstOrDefault(c => c.CategoryId == request.CategoryId);
        
        var expense = new Expense
        {
            ExpenseId = _nextExpenseId++,
            UserId = userId,
            UserName = "Alice Example",
            CategoryId = request.CategoryId,
            CategoryName = category?.CategoryName ?? "Unknown",
            StatusId = 1,
            StatusName = "Draft",
            AmountMinor = (int)(request.Amount * 100),
            Currency = "GBP",
            ExpenseDate = request.ExpenseDate,
            Description = request.Description,
            CreatedAt = DateTime.Now
        };

        _expenses.Add(expense);
        return Task.FromResult(expense);
    }

    public Task<Expense> SubmitExpenseAsync(int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        expense.StatusId = 2;
        expense.StatusName = "Submitted";
        expense.SubmittedAt = DateTime.Now;

        return Task.FromResult(expense);
    }

    public Task<Expense> ApproveExpenseAsync(int expenseId, int reviewerId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        expense.StatusId = 3;
        expense.StatusName = "Approved";
        expense.ReviewedBy = reviewerId;
        expense.ReviewedByName = "Bob Manager";
        expense.ReviewedAt = DateTime.Now;

        return Task.FromResult(expense);
    }

    public Task<Expense> RejectExpenseAsync(int expenseId, int reviewerId)
    {
        var expense = _expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        expense.StatusId = 4;
        expense.StatusName = "Rejected";
        expense.ReviewedBy = reviewerId;
        expense.ReviewedByName = "Bob Manager";
        expense.ReviewedAt = DateTime.Now;

        return Task.FromResult(expense);
    }

    public Task<List<ExpenseCategory>> GetCategoriesAsync()
    {
        return Task.FromResult(_categories.Where(c => c.IsActive).ToList());
    }

    public Task<List<ExpenseStatus>> GetStatusesAsync()
    {
        return Task.FromResult(_statuses.ToList());
    }
}
