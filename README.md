![Header image](https://github.com/DougChisholm/App-Mod-Assist/blob/main/repo-header.png)

# Expense Management System - Modernized Application

A modern cloud-native expense management application built with ASP.NET Core 8.0 and deployed to Azure App Service. This application was generated from legacy screenshots and database schema using GitHub Copilot.

## 🚀 Quick Start

### Prerequisites
- Azure CLI installed and authenticated (`az login`)
- Azure subscription with appropriate permissions
- Git installed

### Deployment Steps

1. **Fork and Clone this Repository**
   ```bash
   git clone <your-fork-url>
   cd AMAForkTue1508
   ```

2. **Login to Azure**
   ```bash
   az login
   az account set --subscription <your-subscription-id>
   ```

3. **Deploy the Application**
   ```bash
   chmod +x deploy.sh
   ./deploy.sh
   ```

4. **Access Your Application**
   
   After deployment completes, navigate to the URL shown in the output:
   ```
   https://<your-app-name>.azurewebsites.net/Index
   ```
   
   ⚠️ **Important**: Navigate to `/Index` not just the root URL!

## 📋 Features

### Current Features (Default Deployment)
- ✅ **Modern UI** - Clean, responsive design using Bootstrap 5
- ✅ **Expense Management** - Add, view, and filter expenses
- ✅ **Manager Approval Workflow** - Review and approve/reject expenses
- ✅ **REST API** - Full CRUD operations on expenses
- ✅ **Swagger Documentation** - Interactive API documentation at `/swagger`
- ✅ **Dummy Data** - Pre-populated sample expenses (no database required)

### Optional Features (Enable with `INCLUDE_CHAT_UI=true`)
- 💬 **AI Chat Assistant** - Natural language interface powered by Azure OpenAI GPT-4o
- 🔍 **RAG Pattern** - Retrieval-Augmented Generation for contextual responses
- 🔗 **Function Calling** - AI can directly interact with expense APIs
- 🔐 **Managed Identity** - Secure authentication to Azure services

## 📱 Application Pages

1. **My Expenses** (`/Index`) - View all expenses with filtering
2. **Add Expense** (`/AddExpense`) - Submit new expense claims
3. **Approve Expenses** (`/ApproveExpenses`) - Manager view for approvals
4. **Chat Assistant** (`/Chat`) - AI-powered expense assistant (when enabled)
5. **API Documentation** (`/swagger`) - Interactive Swagger UI

## 🔧 Configuration

### Basic Deployment (No Chat UI)
The default configuration deploys only the App Service with dummy data:

```bash
# In deploy.sh
INCLUDE_CHAT_UI="false"  # Default
```

**Cost**: Free tier (F1 App Service Plan)

### Advanced Deployment (With Chat UI)
To enable the AI chat assistant:

```bash
# In deploy.sh
INCLUDE_CHAT_UI="true"
```

**Additional Resources Deployed**:
- Azure OpenAI Service (S0 SKU, Sweden Central, GPT-4o model)
- Azure AI Search (Basic SKU)
- Cognitive Services (S0 SKU)

**Cost**: S0/Basic tier services (estimated ~£100-150/month)

### Customization

Edit these files to customize your deployment:

- `deploy.sh` - Resource group name, location, feature flags
- `infrastructure/app-service.bicep` - App Service configuration
- `infrastructure/genai-resources.bicep` - GenAI settings
- `ExpenseApp/appsettings.json` - Application configuration

## 🏗️ Architecture

See [ARCHITECTURE.md](ARCHITECTURE.md) for detailed architecture diagram and data flow.

### Technology Stack
- **Frontend**: ASP.NET Core 8.0 Razor Pages, Bootstrap 5
- **Backend**: ASP.NET Core Web API
- **Cloud**: Azure App Service (Linux)
- **AI**: Azure OpenAI (GPT-4o), Azure AI Search
- **Authentication**: Managed Identity

## 📦 What's Included

```
├── deploy.sh                          # Main deployment script
├── app.zip                            # Compiled application package
├── infrastructure/
│   ├── app-service.bicep             # App Service IaC
│   └── genai-resources.bicep         # GenAI resources IaC
├── ExpenseApp/                        # ASP.NET Core application
│   ├── Controllers/                   # API controllers
│   ├── Models/                        # Data models
│   ├── Services/                      # Business logic
│   └── Pages/                         # Razor Pages
├── chatui/
│   └── RAG/                           # RAG context files
├── Database-Schema/
│   └── database_schema.sql           # Reference DB schema
├── Legacy-Screenshots/                # Original app screenshots
├── GenAISettings.md                   # GenAI configuration guide
└── ARCHITECTURE.md                    # Architecture diagram

```

## 🔌 API Endpoints

### Expenses API (`/api/expenses`)

- `GET /api/expenses` - Get all expenses
- `GET /api/expenses/{id}` - Get expense by ID
- `GET /api/expenses/status/{status}` - Get expenses by status
- `GET /api/expenses/user/{userId}` - Get expenses by user
- `POST /api/expenses` - Create new expense
- `POST /api/expenses/{id}/submit` - Submit expense for approval
- `POST /api/expenses/{id}/approve` - Approve expense
- `POST /api/expenses/{id}/reject` - Reject expense
- `GET /api/expenses/categories` - Get all categories
- `GET /api/expenses/statuses` - Get all statuses

Full API documentation available at `/swagger` after deployment.

## 🎯 Use Cases

### Employee Workflow
1. Navigate to "Add Expense"
2. Enter amount, date, category, and description
3. Click "Submit" - expense is automatically submitted for approval
4. View status on "My Expenses" page

### Manager Workflow
1. Navigate to "Approve Expenses"
2. Review pending expenses
3. Click "Approve" or "Reject" for each expense

### Developer Workflow
1. Navigate to `/swagger` to explore and test APIs
2. Use API endpoints in custom applications
3. Integrate with chat assistant for natural language interaction

## 🔒 Security Considerations

### Current Implementation (POC)
- ✅ HTTPS enforced
- ✅ Managed Identity for Azure services
- ✅ Minimum TLS 1.2
- ✅ No hardcoded secrets
- ⚠️ Uses dummy data (no database)
- ⚠️ No authentication/authorization
- ⚠️ Free tier has limitations

### Production Readiness
Before using in production, implement:
- [ ] Azure AD/Entra ID authentication
- [ ] Role-based access control (RBAC)
- [ ] Azure SQL Database with proper schema
- [ ] Application Insights for monitoring
- [ ] Key Vault for sensitive configuration
- [ ] Premium tier App Service Plan
- [ ] WAF (Web Application Firewall)
- [ ] Backup and disaster recovery
- [ ] CI/CD pipeline

## 📸 Screenshots

Modern UI screenshots are available in the `Modern-Screenshots/` folder (generated after running the application).

## 🛠️ Development

### Local Development

```bash
cd ExpenseApp
dotnet restore
dotnet run
```

Navigate to `https://localhost:5001/Index`

### Building Deployment Package

```bash
cd ExpenseApp
dotnet publish -c Release -o ./publish
cd publish
zip -r ../../app.zip .
```

## 📚 Additional Documentation

- [GenAI Settings Guide](GenAISettings.md) - Configuration for Azure OpenAI
- [Architecture Diagram](ARCHITECTURE.md) - System architecture and data flow
- [Database Schema](Database-Schema/database_schema.sql) - Reference database structure

## 🤝 Contributing

This is a template repository for demonstrating app modernization. To test changes:

1. Fork this repository (use a unique name, not containing "App-Mod-Assist")
2. Make your changes
3. Test with GitHub Copilot agent: "modernise my app"
4. Deploy and verify

## 📄 License

See [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

Generated using GitHub Copilot from legacy application screenshots and database schema, demonstrating AI-assisted application modernization.

---

**Need Help?** Check the `/swagger` endpoint for API documentation or enable the Chat UI for AI assistance!

