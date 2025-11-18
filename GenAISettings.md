# GenAI Settings Configuration

This file contains the configuration settings for the Azure OpenAI and related GenAI resources.

## Azure OpenAI Settings

- **Endpoint**: Your Azure OpenAI endpoint URL (format: https://your-resource-name.openai.azure.com/)
- **API Key**: Your Azure OpenAI API key (retrieve from Azure Portal or use Managed Identity)
- **Deployment Name**: gpt-4o
- **Model**: gpt-4o
- **API Version**: 2024-02-01

## Azure AI Search Settings (for RAG)

- **Endpoint**: Your Azure AI Search endpoint URL (format: https://your-search-service.search.windows.net)
- **API Key**: Your Azure AI Search admin key (retrieve from Azure Portal or use Managed Identity)
- **Index Name**: expenses-index

## Cognitive Services Settings

- **Endpoint**: Your Cognitive Services endpoint URL
- **API Key**: Your Cognitive Services API key (or use Managed Identity)

## Using Managed Identity

When deployed to Azure App Service with Managed Identity enabled, the application can authenticate
to Azure OpenAI and other services without using API keys. Set the `USE_MANAGED_IDENTITY` 
environment variable to `true` to enable this.

## Configuration in appsettings.json

Add the following to your appsettings.json:

```json
{
  "GenAI": {
    "OpenAI": {
      "Endpoint": "https://your-aoai-resource.openai.azure.com/",
      "DeploymentName": "gpt-4o",
      "UseManagedIdentity": true
    },
    "Search": {
      "Endpoint": "https://your-search-service.search.windows.net",
      "IndexName": "expenses-index",
      "UseManagedIdentity": true
    },
    "EnableChatUI": false
  }
}
```

Set `EnableChatUI` to `true` to enable the chat interface.
