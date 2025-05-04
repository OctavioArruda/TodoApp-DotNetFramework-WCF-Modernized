# Migration Tips: .NET Framework WCF to .NET 8 CoreWCF

This document highlights key areas and considerations when migrating a .NET Framework WCF application, like the TodoApp demo, to .NET 8 using CoreWCF. The goal is to leverage modern .NET features like built-in Dependency Injection and the Generic Host.

## Key Migration Areas

1.  **Dependency Injection (DI):**
    * **.NET Framework WCF:** No built-in DI. Requires manual instance creation in host code (`ServiceHost(instance, ...)`) or custom `ServiceHostFactory` for `.svc` hosting.
    * **CoreWCF (.NET 8):** Integrates with built-in `Microsoft.Extensions.DependencyInjection`. Register services and dependencies in `IServiceCollection` (`services.AddTransient<ITodoService, TodoService>();`). CoreWCF resolves instances automatically. `[ServiceBehavior(InstanceContextMode = ...)]` is replaced by DI lifetime registration (Transient ≈ PerCall, Scoped ≈ PerSession, Singleton ≈ Single).

2.  **Hosting Model:**
    * **.NET Framework WCF:** Uses IIS/ASP.NET with `.svc` files (requires `Web.config`, `ServiceHostFactory` for DI) or self-hosting with `ServiceHost` in executables (`App.config`, manual DI instance creation).
    * **CoreWCF (.NET 8):** Hosted within standard .NET Generic Host or ASP.NET Core Host. **No `.svc` files**. Configuration is code-based (`Program.cs`/`Startup.cs`), defining endpoints directly in the host pipeline.
    * **Migration:** Create a new .NET 8 host project (Console or ASP.NET Core Web API). Configure CoreWCF using `AddCoreWCF()`, `UseServiceModel()`, and endpoint/binding definitions in code.

3.  **Configuration:**
    * **.NET Framework WCF:** Primarily XML in `Web.config` or `App.config` (`<system.serviceModel>`).
    * **CoreWCF (.NET 8):** Primarily code-based configuration in the host builder.
    * **Migration:** Translate XML configurations (endpoints, bindings, behaviors) into equivalent C# code in the CoreWCF host startup. This is often a significant manual translation effort.

4.  **Bindings:**
    * **.NET Framework WCF:** Supports many bindings (BasicHttp, WSHttp, NetTcp, NetNamedPipe, etc.).
    * **CoreWCF (.NET 8):** Supports a subset, including BasicHttp, WSHttp, and NetTcp.
    * **Migration:** Verify chosen bindings are supported. Configure them programmatically in the CoreWCF host.

5.  **Client Applications:**
    * **.NET Framework WCF Client:** Existing .NET Framework WCF clients (using `svcutil.exe` generated proxies and `App.config` can often call CoreWCF services. You might just need to update the client's `App.config` or service reference to point to the new CoreWCF endpoint address.
    * **Modern .NET Client:** For new or modernized clients, you might choose different communication patterns (e.g., `HttpClient` for HTTP-based services, gRPC for RPC) or use modern WCF client patterns (`ChannelFactory` or DI-based client builders configured in code).
    * **Migration:** Decide whether to keep the existing WCF client proxy (simplest initially) or modernize the client communication code/configuration. Client configuration shifts from XML (`App.config`) to code.

This file summarizes the key architectural shifts demonstrated by your setup, providing valuable points for your migration narrative.
