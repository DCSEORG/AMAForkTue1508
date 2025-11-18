// Managed Identity Configuration for Azure SQL Database
// This script creates a user in the database for the App Service managed identity

@description('Name of the Azure SQL Server')
param sqlServerName string

@description('Name of the Azure SQL Database')
param databaseName string = 'ExpenseDB'

@description('Managed Identity Principal ID from App Service')
param appServicePrincipalId string

@description('App Service Managed Identity Object ID')
param appServiceObjectId string

@description('SQL Server Administrator Login')
param adminLogin string

@description('SQL Server Administrator Password')
@secure()
param adminPassword string

@description('Location for all resources')
param location string = resourceGroup().location

// Azure SQL Server
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
  }
}

// Azure SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: databaseName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
  }
}

// Firewall rule to allow Azure services
resource firewallRule 'Microsoft.Sql/servers/firewallRules@2022-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// Note: Creating SQL users with managed identity requires running T-SQL commands
// After deploying this infrastructure, run the following SQL script as the admin:
//
// CREATE USER [webapp-expense-XXXXX] FROM EXTERNAL PROVIDER;
// ALTER ROLE db_datareader ADD MEMBER [webapp-expense-XXXXX];
// ALTER ROLE db_datawriter ADD MEMBER [webapp-expense-XXXXX];
// ALTER ROLE db_ddladmin ADD MEMBER [webapp-expense-XXXXX];
//
// Replace XXXXX with the actual app service name

output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output databaseName string = sqlDatabase.name
output sqlConnectionString string = 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Database=${databaseName};Authentication=Active Directory Managed Identity;Encrypt=True;TrustServerCertificate=False;'
