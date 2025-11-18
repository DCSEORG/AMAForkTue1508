-- SQL Script to Create Managed Identity User for App Service
-- Run this script as the Azure SQL Server administrator after deploying infrastructure

-- Prerequisites:
-- 1. App Service with System-Assigned Managed Identity must be deployed
-- 2. You must be connected to the Azure SQL Database as the admin user
-- 3. The database must be created

-- Replace [YOUR-WEBAPP-NAME] with the actual name of your App Service
-- You can find this in the Azure Portal or in the deployment output

-- Step 1: Create a user for the App Service managed identity
CREATE USER [YOUR-WEBAPP-NAME] FROM EXTERNAL PROVIDER;
GO

-- Step 2: Grant the user permissions to read and write data
ALTER ROLE db_datareader ADD MEMBER [YOUR-WEBAPP-NAME];
ALTER ROLE db_datawriter ADD MEMBER [YOUR-WEBAPP-NAME];
GO

-- Step 3: (Optional) Grant DDL admin if the app needs to create/modify schema
ALTER ROLE db_ddladmin ADD MEMBER [YOUR-WEBAPP-NAME];
GO

-- Step 4: Verify the user was created
SELECT name, type_desc, authentication_type_desc
FROM sys.database_principals
WHERE name = '[YOUR-WEBAPP-NAME]';
GO

-- Step 5: (Optional) Grant execute permissions if using stored procedures
-- GRANT EXECUTE TO [YOUR-WEBAPP-NAME];
-- GO

PRINT 'Managed Identity user created successfully!';
PRINT 'The App Service can now connect using its managed identity.';
PRINT '';
PRINT 'Connection string format:';
PRINT 'Server=tcp:YOUR-SERVER.database.windows.net,1433;Database=YOUR-DATABASE;Authentication=Active Directory Managed Identity;Encrypt=True;';
GO
