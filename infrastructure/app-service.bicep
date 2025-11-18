// Azure App Service Infrastructure
// Creates App Service Plan and Web App in UK South region with low-cost dev SKU

@description('Name of the resource group')
param resourceGroupName string = 'rg-expense-app-dev'

@description('Location for all resources')
param location string = 'uksouth'

@description('Name of the App Service Plan')
param appServicePlanName string = 'asp-expense-app-dev'

@description('Name of the Web App')
param webAppName string = 'webapp-expense-${uniqueString(resourceGroup().id)}'

@description('App Service Plan SKU')
param sku string = 'F1'

@description('Enable managed identity for the Web App')
param enableManagedIdentity bool = true

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: sku
    tier: 'Free'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

// Web App
resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: webAppName
  location: location
  identity: enableManagedIdentity ? {
    type: 'SystemAssigned'
  } : null
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: false
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      netFrameworkVersion: 'v8.0'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
      ]
    }
  }
}

output webAppName string = webApp.name
output webAppUrl string = 'https://${webApp.properties.defaultHostName}'
output webAppPrincipalId string = enableManagedIdentity ? webApp.identity.principalId : ''
