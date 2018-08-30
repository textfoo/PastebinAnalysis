# Pastebin Analysis Project
An application to scan pastbin, cache results, scan items, and hopefully identify interesting trends. 

**This solution assumes a pastebin pro subscription with a whitelist configuration**

## Stack : 
ASP.NET Core 2.0/2.1 Console Application
MongoDB Backend Database

## Dependencies 
*Microsoft.Extensions.DependencyInjections* 

*Microsoft.Extensions.Configuration* 

*Microsoft.Extensions.Configuration.Json* 

*Microsoft.Extensions.Logging* 

*Microsoft.Extensions.Logging.Configuration* 

*Microsoft.Extensions.Logging.Console* 

*Microsoft.Extensions.Logging.Debug*

## Internal Dependencies
*HttpService.csproj* 
> Manages Http connections 

*MongoDB.csproj*
> Manages database context 

*PastebinService.csproj*
> Manages functions related to Pastebin (contains dependency to HttpService) 
