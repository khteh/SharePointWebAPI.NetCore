# SharePointWebAPI.NetCore
SharePointWebAPI using .Net Core 2.0 and Microsoft.SharePoint.Client DLLs.

Description:

This application is using .Net Core 2.0 and Microsoft.SharePoint.Client DLLs. It implements the following features:

* Authenticate with SharePoint online
* List SharePoint site collection templates
* List SharePoint site collections
* Create new SharePoint site collection
* Delete SharePoint site collection
* Create new SharePoint sites
* Delete SharePoint sites

Build Steps:

* Open the solution file in Visual Studio
* Right click the project and choose "Manage User Secrets"
* Enter the following values into the secret file:

    "Authentication:SharePoint:Username": "<SharePoint Online login id/email>",

    "Authentication:SharePoint:Password": "<SharePoint Online login password>",

* Save the file
* Build and run the solution. This can usually be achieved by hitting "F5" key.
* To run the Unit Test project, update the UserSecretsId value in SharePointWebAPIUnitTest.csproj in a text editor with the value in SharePointWebAPI.NetCore.csproj

Swagger:
* Browse to http://localhost:<port>/swagger/ to view swagger documentation of the API

Known Issues:

* Microsoft.SharePoint.Client currently does not support tenant administration when it is used with .Net Core 2.0: https://social.technet.microsoft.com/Forums/en-US/3dacdc8f-a819-4451-8b2c-10f8f14e832b/sharepoint-online-client-components-sdk-does-not-work-with-net-core-20?forum=sharepointdevelopment