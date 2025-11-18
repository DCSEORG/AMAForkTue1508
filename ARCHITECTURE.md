# Azure Services Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                        Expense Management System                         │
│                         Azure Architecture                               │
└─────────────────────────────────────────────────────────────────────────┘

┌──────────────┐
│   End User   │
└──────┬───────┘
       │
       │ HTTPS
       ▼
┌──────────────────────────────────────────────────────────────────┐
│                      Azure App Service                           │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  ASP.NET Core 8.0 Razor Pages Application                 │  │
│  │  - Expense Management UI (Index, Add, Approve pages)      │  │
│  │  - REST APIs (/api/expenses)                              │  │
│  │  - Swagger/OpenAPI Documentation                          │  │
│  │  - Chat UI (optional, if includeChatUI=true)             │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                  │
│  Features:                                                       │
│  - System-Assigned Managed Identity ✓                           │
│  - HTTPS Only                                                    │
│  - Free/F1 SKU (Development)                                    │
│  - Location: UK South                                           │
└──────────────┬───────────────────────────────────────────────────┘
               │
               │ Uses Managed Identity (when includeChatUI=true)
               │
               ▼
┌──────────────────────────────────────────────────────────────────┐
│              Azure OpenAI Service (Optional)                     │
│  Only deployed when includeChatUI=true                          │
│                                                                  │
│  - Model: GPT-4o                                                │
│  - Deployment Name: gpt-4o                                      │
│  - Location: Sweden Central (for GPT-4o availability)           │
│  - SKU: S0                                                      │
│  - Function Calling enabled for API integration                 │
└──────────────┬───────────────────────────────────────────────────┘
               │
               │ Uses for RAG pattern
               │
               ▼
┌──────────────────────────────────────────────────────────────────┐
│         Azure AI Search (Optional)                               │
│  Only deployed when includeChatUI=true                          │
│                                                                  │
│  - Index: expenses-index                                        │
│  - Location: UK South                                           │
│  - SKU: Basic                                                   │
│  - Used for: Retrieval-Augmented Generation (RAG)              │
│  - Contains: Expense context, business rules, FAQs             │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│        Cognitive Services (Optional)                             │
│  Only deployed when includeChatUI=true                          │
│                                                                  │
│  - Multi-service Cognitive Services account                     │
│  - Location: UK South                                           │
│  - SKU: S0                                                      │
│  - Used for: Additional AI capabilities                        │
└──────────────────────────────────────────────────────────────────┘


Data Flow:
──────────

1. User accesses App Service via HTTPS
2. App Service serves Razor Pages UI and REST APIs
3. All data is currently stored in-memory (dummy data service)
4. If Chat UI enabled:
   a. User queries sent to Azure OpenAI (GPT-4o)
   b. OpenAI uses function calling to invoke expense APIs
   c. Azure AI Search provides context via RAG pattern
   d. Response returned to user via Chat UI


Deployment Components:
─────────────────────

Infrastructure as Code:
- infrastructure/app-service.bicep        → Deploys App Service + Plan
- infrastructure/genai-resources.bicep    → Deploys OpenAI, Search, Cog Services

Application Code:
- app.zip                                 → Contains compiled .NET application

Deployment Script:
- deploy.sh                               → Orchestrates all deployments


Key Configuration:
─────────────────

Default (Chat UI Disabled):
- Only App Service deployed
- Uses dummy data service
- No external dependencies
- Cost: Free tier

With Chat UI Enabled (set INCLUDE_CHAT_UI=true):
- App Service + OpenAI + AI Search + Cognitive Services
- GenAI chat assistant available
- Function calling integrated with APIs
- Cost: S0/Basic tier services


Security Features:
────────────────

✓ System-Assigned Managed Identity on App Service
✓ HTTPS only enforcement
✓ No hardcoded secrets (uses Managed Identity)
✓ Minimum TLS version 1.2
✓ FTPS disabled
```
