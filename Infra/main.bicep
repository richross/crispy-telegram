@description('Location for all resources.')
param location string = 'eastus2' //resourceGroup().location

@description('Name of the App Service Plan')
param appServicePlanName string = 'asp-${uniqueString(resourceGroup().id)}'

@description('Name of the App Service')
param appServiceName string = 'crispy-telegram'

@description('Container image to deploy')
param containerImage string = 'mcr.microsoft.com/appsvc/staticsite:latest'

@description('Container registry URL')
param containerRegistryUrl string = ''

@description('Container registry username')
param containerRegistryUsername string = ''

@allowed([
  'None'
  'PrivateRegistry'
])
@description('Container registry authentication type')
param containerRegistryAuthType string = 'None'

@secure()
@description('Container registry password')
param containerRegistryPassword string = ''

// App Service Plan - Premium V3 P1v3 SKU supports sidecars
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'P0v3'
    tier: 'PremiumV3'
    size: 'P0v3'
    family: 'Pv3'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true // Required for Linux
  }
}

// App Service
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|${containerImage}'
      acrUseManagedIdentityCredentials: false
      alwaysOn: true
      appSettings: [
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'false'
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_URL'
          value: !empty(containerRegistryUrl) ? 'https://${containerRegistryUrl}' : ''
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_USERNAME'
          value: containerRegistryUsername
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_PASSWORD'
          value: containerRegistryPassword
        }
        {
          name: 'WEBSITES_PORT'
          value: '8080'
        }
      ]
    }
  }
}

// Output the website URL
output websiteUrl string = 'https://${appService.properties.defaultHostName}'
