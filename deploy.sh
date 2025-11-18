#!/bin/bash

# Main deployment script for Expense Management Application
# This script deploys all Azure infrastructure and the application

set -e

# Configuration
RESOURCE_GROUP="rg-expense-app-dev"
LOCATION="uksouth"
INCLUDE_CHAT_UI="false"  # Set to "true" to enable GenAI chat UI

echo "======================================"
echo "Expense Management App Deployment"
echo "======================================"
echo "Resource Group: $RESOURCE_GROUP"
echo "Location: $LOCATION"
echo "Include Chat UI: $INCLUDE_CHAT_UI"
echo "======================================"

# Create resource group
echo "Creating resource group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

# Deploy App Service infrastructure
echo "Deploying App Service..."
az deployment group create \
  --resource-group $RESOURCE_GROUP \
  --template-file infrastructure/app-service.bicep \
  --parameters location=$LOCATION

# Get the web app name from deployment output
WEB_APP_NAME=$(az deployment group show \
  --resource-group $RESOURCE_GROUP \
  --name app-service \
  --query properties.outputs.webAppName.value \
  --output tsv)

echo "Web App Name: $WEB_APP_NAME"

# Deploy GenAI resources if enabled
if [ "$INCLUDE_CHAT_UI" = "true" ]; then
  echo "Deploying GenAI resources..."
  az deployment group create \
    --resource-group $RESOURCE_GROUP \
    --template-file infrastructure/genai-resources.bicep \
    --parameters location=$LOCATION includeChatUI=true
  
  echo "GenAI resources deployed successfully"
fi

# Deploy application code
if [ -f "app.zip" ]; then
  echo "Deploying application code..."
  az webapp deploy \
    --resource-group $RESOURCE_GROUP \
    --name $WEB_APP_NAME \
    --src-path ./app.zip \
    --type zip
  
  echo "Application deployed successfully!"
  
  WEB_APP_URL=$(az deployment group show \
    --resource-group $RESOURCE_GROUP \
    --name app-service \
    --query properties.outputs.webAppUrl.value \
    --output tsv)
  
  echo ""
  echo "======================================"
  echo "Deployment Complete!"
  echo "======================================"
  echo "Application URL: ${WEB_APP_URL}/Index"
  echo ""
  echo "IMPORTANT: Navigate to ${WEB_APP_URL}/Index to view the application"
  echo "======================================"
else
  echo "Warning: app.zip not found. Please build the application first."
  echo "Run: dotnet publish -c Release and create app.zip"
fi
