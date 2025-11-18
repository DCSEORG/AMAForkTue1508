// Azure OpenAI and Cognitive Services Infrastructure
// Creates Azure OpenAI with GPT-4o model in Sweden region

@description('Location for all resources')
param location string = 'uksouth'

@description('Location for Azure OpenAI - must be Sweden for GPT-4o')
param openAILocation string = 'swedencentral'

@description('Enable Chat UI and GenAI functionality')
param includeChatUI bool = false

@description('Name of the Azure OpenAI service')
param openAIName string = 'aoai-expense-${uniqueString(resourceGroup().id)}'

@description('Name of the Cognitive Services account')
param cognitiveServicesName string = 'cog-expense-${uniqueString(resourceGroup().id)}'

@description('Name of the Azure AI Search service for RAG')
param searchServiceName string = 'srch-expense-${uniqueString(resourceGroup().id)}'

@description('SKU for Azure OpenAI')
param openAISku string = 'S0'

@description('SKU for Cognitive Services')
param cognitiveServicesSku string = 'S0'

@description('SKU for Azure AI Search')
param searchServiceSku string = 'basic'

// Azure OpenAI Service
resource openAI 'Microsoft.CognitiveServices/accounts@2023-05-01' = if (includeChatUI) {
  name: openAIName
  location: openAILocation
  kind: 'OpenAI'
  sku: {
    name: openAISku
  }
  properties: {
    customSubDomainName: openAIName
    publicNetworkAccess: 'Enabled'
  }
}

// Deploy GPT-4o model
resource gpt4oDeployment 'Microsoft.CognitiveServices/accounts/deployments@2023-05-01' = if (includeChatUI) {
  parent: openAI
  name: 'gpt-4o'
  sku: {
    name: 'Standard'
    capacity: 10
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-4o'
      version: '2024-05-13'
    }
  }
}

// Azure AI Search for RAG
resource searchService 'Microsoft.Search/searchServices@2023-11-01' = if (includeChatUI) {
  name: searchServiceName
  location: location
  sku: {
    name: searchServiceSku
  }
  properties: {
    replicaCount: 1
    partitionCount: 1
    hostingMode: 'default'
    publicNetworkAccess: 'enabled'
  }
}

// Cognitive Services for additional AI capabilities
resource cognitiveServices 'Microsoft.CognitiveServices/accounts@2023-05-01' = if (includeChatUI) {
  name: cognitiveServicesName
  location: location
  kind: 'CognitiveServices'
  sku: {
    name: cognitiveServicesSku
  }
  properties: {
    customSubDomainName: cognitiveServicesName
    publicNetworkAccess: 'Enabled'
  }
}

output openAIEndpoint string = includeChatUI ? openAI.properties.endpoint : ''
output openAIName string = includeChatUI ? openAI.name : ''
output searchServiceEndpoint string = includeChatUI ? 'https://${searchService.name}.search.windows.net' : ''
output searchServiceName string = includeChatUI ? searchService.name : ''
output cognitiveServicesEndpoint string = includeChatUI ? cognitiveServices.properties.endpoint : ''
output includeChatUI bool = includeChatUI
