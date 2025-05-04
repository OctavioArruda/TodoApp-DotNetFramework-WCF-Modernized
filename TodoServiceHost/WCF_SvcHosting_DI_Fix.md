# Guide: Fixing "The service type provided could not be loaded as a service because it does not have a default (parameter-less) constructor." in .NET Framework WCF (.svc Hosting)

This guide explains a common error encountered when hosting a WCF service via a `.svc` file in .NET Framework (IIS/ASP.NET) after modifying the service class to use constructor-based Dependency Injection (DI), and provides the standard solution using a custom `ServiceHostFactory`.

## The Error

You encounter this error when attempting to access your `.svc` endpoint (e.g., Browse to it or adding a service reference), and the service class (`YourServiceType`) does not have a public parameterless constructor, but instead requires parameters (e.g., for injected dependencies).

The error message typically looks like this in the browser or in the output of the "Add Service Reference" tool:

And often includes a `System.InvalidOperationException` or `System.ServiceModel.ServiceActivationException` in the stack trace pointing back to the service activation process.

## Context

This happens in .NET Framework WCF when you:

1.  Host a WCF service using a `.svc` file in an IIS/ASP.NET application (like a "WCF Service Application" project template).
2.  Modify your service implementation class (`YourServiceType`) to accept dependencies via its constructor (Constructor Injection), leading to the removal or absence of a public parameterless constructor.
3.  The default WCF `.svc` activation process (which uses an internal `ServiceHostFactory`) attempts to create an instance of `YourServiceType` by calling `new YourServiceType()`, which fails because that constructor no longer exists.

## Problem

The standard `.svc` hosting mechanism doesn't automatically know how to resolve dependencies or use parameterized constructors when creating service instances. It relies on a parameterless constructor or a custom factory to handle instance creation.

## Solution: Implement a Custom `ServiceHostFactory`

The standard solution in .NET Framework WCF `.svc` hosting is to create a custom class that inherits from `System.ServiceModel.Activation.ServiceHostFactory`. This factory class is responsible for creating the `ServiceHost` and, crucially, providing it with an instance of your service class (which the factory creates, manually resolving dependencies or using a DI container).

You then configure your `.svc` file to use this custom factory instead of the default one.

### Step 1: (Prerequisite) Ensure Service Class is Ready for DI

Your service class (`TodoService` in our example) should already be set up for constructor injection and have the `[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]` attribute if you plan to provide a single instance to the `ServiceHost` constructor within the factory.

```csharp
using System.ServiceModel; // Required for ServiceBehavior and InstanceContextMode

// ... other usings

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // Required if passing an instance to ServiceHost
public class YourServiceType : IYourServiceContract // Replace with your class and interface names
{
    // Constructor that requires parameters (e.g., injected dependencies)
    public YourServiceType(IDependency dependency) // Replace with your actual dependency
    {
        // ... use dependency ...
    }

    // Make sure there is NO public parameterless constructor here!
    // public YourServiceType() { /* This constructor is intentionally missing */ }

    // ... service operations ...
}