# Expense Management System Context

This file provides contextual information for the AI chat assistant to help users manage their expenses effectively.

## System Overview

The Expense Management System allows employees to:
- Submit expense claims for reimbursement
- Track the status of their expense claims
- View their expense history

Managers can:
- Review submitted expense claims
- Approve or reject expense claims
- View all expenses for their team

## Expense Categories

The system supports the following expense categories:
1. **Travel** - Transportation costs including taxis, trains, buses, flights
2. **Meals** - Business meals and client entertainment
3. **Accommodation** - Hotel stays and lodging
4. **Supplies** - Office supplies, stationery, equipment
5. **Other** - Any other business-related expenses

## Expense Status

Expenses can have the following statuses:
- **Draft** - Expense created but not yet submitted
- **Submitted** - Expense submitted for manager review
- **Approved** - Expense approved by manager, ready for reimbursement
- **Rejected** - Expense rejected by manager

## Business Rules

1. All expenses must include:
   - Amount (in GBP)
   - Date of expense
   - Category
   - Description (optional but recommended)

2. Expenses should be submitted within 30 days of incurring the cost

3. Receipts should be attached for expenses over £25

4. Manager approval is required for all submitted expenses

## Common User Questions

**Q: How do I submit an expense?**
A: Navigate to the "Add Expense" page, fill in the amount, date, category, and description, then click Submit.

**Q: How long does approval take?**
A: Managers typically review expenses within 1-2 business days.

**Q: What happens if my expense is rejected?**
A: You can view the reason for rejection and resubmit with corrections if needed.

**Q: Can I edit a submitted expense?**
A: Once submitted, expenses cannot be edited. If changes are needed, contact your manager.

## API Functions Available

The chat assistant can help you:
- Create new expense entries
- View your expense history
- Check the status of specific expenses
- Filter expenses by category or status
- Submit expenses for approval

## Sample Queries

- "Show me all my submitted expenses"
- "Create a new travel expense for £50"
- "What expenses are pending approval?"
- "Show me all approved expenses from last month"
