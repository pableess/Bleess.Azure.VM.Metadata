# Bleess.Azure.VM.Metadata
Strongly typed dotnet client for Azure VM instance metadata service

See MS documentation on VM metadata and scheduled events
https://docs.microsoft.com/en-us/azure/virtual-machines/windows/instance-metadata-service

## Features
- Query Azure VM Metadata instance service (Instance, Attested, ScheduledEvents)
- Start scheduled events
- Validation of attested metadata

## Usage

Add the nuget package Bleess.Azure.VM.Metadata

This package is meant to be used with Microsoft.Extensions.DependencyInjection

 
 ```csharp
 serviceCollection.AddAzureVmMetadataClient();
 ```
 
 Once registered, instances of IVmMetadataClient can be injected as a dependency.  
