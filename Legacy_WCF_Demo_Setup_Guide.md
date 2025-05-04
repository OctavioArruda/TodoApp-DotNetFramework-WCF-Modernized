# Legacy .NET Framework WCF Demo Setup Guide

This document outlines the steps taken to configure the TodoApp .NET Framework 4.7 WCF application with Dependency Injection (DI) and host it with both HTTP and NetTcp endpoints, addressing the challenges encountered during the process. This serves as the "before" state for a migration demo to .NET 8 with CoreWCF.

## 1. Initial State & Goal

* **Initial Code:** A .NET Framework 4.7 WCF Service Library (`TodoServiceLibrary`) with a service implementation (`TodoService`) and a simple client test project (`TodoServiceTestClient`).
* **Problem Area:** The `TodoService` directly instantiated a dependency (`DeprecatedDependencyInjectionDemo`), resulting in tight coupling and making DI difficult.
* **Goal:**
    * Modify `TodoService` to use constructor injection.
    * Host `TodoService` with both a NetTcp endpoint and an HTTP endpoint in .NET Framework 4.7.
    * Configure the `TodoServiceTestClient` to call the service over either endpoint using WCF client proxies.
    * Address specific WCF hosting challenges related to DI in .NET Framework.

## 2. Implementing Constructor Injection

* **Change:** Removed the parameterless constructor from `TodoService` and added a constructor that accepts `DeprecatedDependencyInjectionDemo`.

```csharp
// In TodoServiceLibrary/TodoService.cs
// REMOVE: public TodoService() { }
public class TodoService : ITodoService
{
    private readonly DeprecatedDependencyInjectionDemo _demo;

    public TodoService(DeprecatedDependencyInjectionDemo demo) // Added constructor
    {
        _demo = demo ?? throw new ArgumentNullException(nameof(demo));
        // ... rest of constructor ...
    }
    // ... rest of class ...
}