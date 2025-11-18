# Deployment Verification Checklist

Use this checklist to verify your deployment was successful.

## Pre-Deployment Checks

- [ ] Azure CLI installed and up to date (`az --version`)
- [ ] Logged into Azure (`az login`)
- [ ] Correct subscription selected (`az account show`)
- [ ] Review settings in `deploy.sh`:
  - [ ] Resource group name
  - [ ] Location (default: uksouth)
  - [ ] Chat UI setting (default: false)

## Deployment Process

- [ ] Make deploy.sh executable (`chmod +x deploy.sh`)
- [ ] Run deployment script (`./deploy.sh`)
- [ ] Wait for completion (typically 3-5 minutes without Chat UI, 10-15 with)
- [ ] Note the application URL from output
- [ ] Save the resource group name for cleanup later

## Post-Deployment Verification

### Basic Functionality (All Deployments)

- [ ] Navigate to `https://<your-app>.azurewebsites.net/Index`
- [ ] Verify Index page loads with expense list
- [ ] Check that dummy data is displayed (4 sample expenses)
- [ ] Test filtering expenses using search box
- [ ] Navigate to Add Expense page (`/AddExpense`)
- [ ] Submit a new expense
- [ ] Verify expense appears in the list
- [ ] Navigate to Approve Expenses page (`/ApproveExpenses`)
- [ ] Verify submitted expenses are shown
- [ ] Test approving an expense
- [ ] Test rejecting an expense
- [ ] Navigate to Swagger docs (`/swagger`)
- [ ] Verify all API endpoints are documented
- [ ] Test an API endpoint from Swagger UI

### API Testing

- [ ] GET /api/expenses - Returns expense list
- [ ] GET /api/expenses/status/Submitted - Returns submitted expenses only
- [ ] POST /api/expenses - Creates new expense (test via Swagger)
- [ ] GET /api/expenses/categories - Returns category list
- [ ] GET /api/expenses/statuses - Returns status list

### Chat UI Testing (If INCLUDE_CHAT_UI=true)

- [ ] Navigate to Chat page (`/Chat`)
- [ ] Verify GenAI resources are deployed in Azure Portal:
  - [ ] Azure OpenAI Service
  - [ ] Azure AI Search
  - [ ] Cognitive Services
- [ ] Check that chat UI displays (not "Chat UI is not enabled" message)
- [ ] Test sending a message
- [ ] Verify response is received

### Azure Resources Verification

#### Basic Deployment
- [ ] Resource Group created
- [ ] App Service Plan created (Free/F1 tier)
- [ ] App Service created
- [ ] App Service has System-Assigned Managed Identity enabled
- [ ] App Service is accessible via HTTPS

#### Advanced Deployment (with Chat UI)
All basic resources plus:
- [ ] Azure OpenAI Service (Sweden Central, S0 tier)
- [ ] GPT-4o model deployed
- [ ] Azure AI Search (Basic tier)
- [ ] Cognitive Services (S0 tier)

## Common Issues & Solutions

### Issue: App Service returns 502/503 error
**Solution**: Wait 2-3 minutes for the app to fully start. Check deployment logs in Azure Portal.

### Issue: Swagger page shows errors
**Solution**: Ensure you're accessing `/swagger` not `/swagger/index.html`. Clear browser cache.

### Issue: Chat UI shows "not enabled" message
**Solution**: 
1. Verify `INCLUDE_CHAT_UI="true"` in deploy.sh
2. Redeploy with `./deploy.sh`
3. Check GenAI resources are deployed in Azure Portal

### Issue: Navigation links don't work
**Solution**: Ensure you're accessing the app at `/Index` not root `/`

### Issue: Can't connect to APIs
**Solution**: Check CORS settings. APIs are accessible from same origin only by default.

## Performance Checks

- [ ] Index page loads in < 2 seconds
- [ ] API responses return in < 500ms
- [ ] No console errors in browser developer tools
- [ ] Mobile responsive design works correctly
- [ ] All images and styles load correctly

## Security Verification

- [ ] App only accessible via HTTPS
- [ ] HTTP automatically redirects to HTTPS
- [ ] Managed Identity is enabled
- [ ] No secrets in application logs
- [ ] Swagger endpoint available (acceptable for POC, disable in production)

## Cleanup

When done testing:
```bash
# Delete all resources
az group delete --name rg-expense-app-dev --yes --no-wait
```

## Cost Estimate

### Basic Deployment (No Chat UI)
- **App Service (F1)**: Free
- **Total**: £0/month

### Advanced Deployment (With Chat UI)
- **App Service (F1)**: Free
- **Azure OpenAI (S0)**: ~£50-80/month
- **Azure AI Search (Basic)**: ~£60/month
- **Cognitive Services (S0)**: ~£10/month
- **Total**: ~£120-150/month

**Note**: Delete resources when not in use to avoid charges!

## Support

For issues:
1. Check Azure Portal deployment logs
2. Review application logs in App Service
3. Verify all endpoints are accessible
4. Check that DNS propagation is complete (can take a few minutes)

## Next Steps

After successful deployment:
1. Capture screenshots for Modern-Screenshots folder
2. Test all functionality thoroughly
3. Review architecture diagram in ARCHITECTURE.md
4. Read GenAI integration guide if using Chat UI
5. Plan production hardening (see README.md security section)
