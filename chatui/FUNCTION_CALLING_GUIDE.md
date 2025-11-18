# GenAI Function Calling Integration Guide

This document explains how the Azure OpenAI chat assistant integrates with the Expense Management APIs using function calling.

## Overview

The chat UI uses Azure OpenAI's function calling feature to allow the AI to interact directly with the expense management APIs. This enables users to perform actions using natural language.

## How It Works

1. **User Input**: User types a natural language query (e.g., "Show me my submitted expenses")
2. **AI Processing**: Azure OpenAI (GPT-4o) analyzes the query
3. **Function Selection**: AI determines which API endpoint(s) to call
4. **API Invocation**: The application calls the selected API with appropriate parameters
5. **Response**: Results are formatted and presented to the user

## Available Functions

The AI assistant can call the following expense management functions:

### 1. Get All Expenses
```json
{
  "name": "get_all_expenses",
  "description": "Retrieves all expenses for the current user",
  "parameters": {
    "type": "object",
    "properties": {},
    "required": []
  }
}
```
Maps to: `GET /api/expenses`

### 2. Get Expenses by Status
```json
{
  "name": "get_expenses_by_status",
  "description": "Retrieves expenses filtered by status (Draft, Submitted, Approved, Rejected)",
  "parameters": {
    "type": "object",
    "properties": {
      "status": {
        "type": "string",
        "enum": ["Draft", "Submitted", "Approved", "Rejected"],
        "description": "The status to filter by"
      }
    },
    "required": ["status"]
  }
}
```
Maps to: `GET /api/expenses/status/{status}`

### 3. Create Expense
```json
{
  "name": "create_expense",
  "description": "Creates a new expense entry",
  "parameters": {
    "type": "object",
    "properties": {
      "amount": {
        "type": "number",
        "description": "The amount in GBP (e.g., 45.50)"
      },
      "expense_date": {
        "type": "string",
        "format": "date",
        "description": "The date of the expense (YYYY-MM-DD)"
      },
      "category_id": {
        "type": "integer",
        "description": "Category: 1=Travel, 2=Meals, 3=Supplies, 4=Accommodation, 5=Other"
      },
      "description": {
        "type": "string",
        "description": "Description of the expense"
      }
    },
    "required": ["amount", "expense_date", "category_id"]
  }
}
```
Maps to: `POST /api/expenses`

### 4. Submit Expense
```json
{
  "name": "submit_expense",
  "description": "Submits a draft expense for manager approval",
  "parameters": {
    "type": "object",
    "properties": {
      "expense_id": {
        "type": "integer",
        "description": "The ID of the expense to submit"
      }
    },
    "required": ["expense_id"]
  }
}
```
Maps to: `POST /api/expenses/{id}/submit`

### 5. Get Categories
```json
{
  "name": "get_categories",
  "description": "Retrieves all available expense categories",
  "parameters": {
    "type": "object",
    "properties": {},
    "required": []
  }
}
```
Maps to: `GET /api/expenses/categories`

## Implementation Approach

### Simple Approach (Current - Demo Only)

The current implementation provides a **demo chat interface** that simulates AI responses. In production, you would:

1. Configure Azure OpenAI client with endpoint and API key/managed identity
2. Define function schemas matching the above specifications
3. Send user messages to Azure OpenAI with function definitions
4. Parse function calls from AI responses
5. Execute the corresponding API calls
6. Return results to the AI for natural language formatting
7. Display formatted response to user

### Using Azure SDK (Production)

For production implementation, use the Azure OpenAI SDK:

```csharp
using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;

// Configure client
var endpoint = new Uri(configuration["GenAI:OpenAI:Endpoint"]);
var credential = new DefaultAzureCredential();
var client = new OpenAIClient(endpoint, credential);

// Define functions
var functions = new List<ChatCompletionsFunctionToolDefinition>
{
    new ChatCompletionsFunctionToolDefinition
    {
        Name = "get_expenses_by_status",
        Description = "Retrieves expenses filtered by status",
        Parameters = BinaryData.FromObjectAsJson(new
        {
            type = "object",
            properties = new
            {
                status = new
                {
                    type = "string",
                    @enum = new[] { "Draft", "Submitted", "Approved", "Rejected" }
                }
            },
            required = new[] { "status" }
        })
    }
    // Add other functions...
};

// Chat completion with functions
var chatCompletionsOptions = new ChatCompletionsOptions
{
    DeploymentName = "gpt-4o",
    Messages =
    {
        new ChatRequestSystemMessage("You are a helpful expense management assistant."),
        new ChatRequestUserMessage(userInput)
    },
    Tools = { functions }
};

var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
```

### Without Additional Framework

The implementation avoids using Semantic Kernel or other heavy frameworks, instead using direct Azure OpenAI SDK calls for simplicity and transparency.

## RAG Integration

The system uses Retrieval-Augmented Generation (RAG) to provide contextual information:

1. **Context Files**: Located in `chatui/RAG/expense-context.md`
2. **Azure AI Search**: Indexes the context files for quick retrieval
3. **Query Enhancement**: User queries are enhanced with relevant context before sending to AI
4. **Better Responses**: AI provides more accurate answers based on business rules and FAQs

## Example Conversations

**User**: "Show me all my submitted expenses"
- Function: `get_expenses_by_status(status="Submitted")`
- Response: "Here are your submitted expenses: [list with amounts and dates]"

**User**: "Create a travel expense for £45.50 from yesterday"
- Function: `create_expense(amount=45.50, expense_date="2024-11-17", category_id=1, description="Travel expense")`
- Response: "I've created a travel expense for £45.50. The expense has been submitted for approval."

**User**: "What categories can I use?"
- Function: `get_categories()`
- Response: "Available categories: Travel, Meals, Supplies, Accommodation, Other"

## Security Considerations

1. **Authentication**: Ensure users can only access their own expenses
2. **Authorization**: Implement role-based access for manager functions
3. **Input Validation**: Validate all AI-generated function parameters
4. **Rate Limiting**: Prevent abuse of the chat interface
5. **Audit Logging**: Track all AI-initiated actions

## Configuration

Set these in `appsettings.json`:

```json
{
  "GenAI": {
    "EnableChatUI": true,
    "OpenAI": {
      "Endpoint": "https://your-resource.openai.azure.com/",
      "DeploymentName": "gpt-4o",
      "UseManagedIdentity": true
    },
    "Search": {
      "Endpoint": "https://your-search.search.windows.net",
      "IndexName": "expenses-index",
      "UseManagedIdentity": true
    }
  }
}
```

## Future Enhancements

- Multi-turn conversations with context retention
- Batch operations (e.g., "Approve all expenses under £50")
- Natural language reports (e.g., "Show me spending by category this month")
- Expense policy checks (e.g., "Is this expense within policy?")
- Receipt analysis using Azure Document Intelligence
