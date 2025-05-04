# WCF to CoreWCF Migration Steps

This document tracks the step-by-step process of migrating the .NET Framework 4.7 WCF TodoApp demo application to .NET 8 using CoreWCF. It details the actions taken in each step and explains the reasoning behind them, linking back to the challenges in the legacy WCF setup.

**Refer to:**
* [`README.md`](../README.md) for the overall project overview and high-level migration plan.
* [`Legacy_WCF_Demo_Setup_Guide.md`](./Legacy_WCF_Demo_Setup_Guide.md) for details on setting up and troubleshooting the original .NET Framework WCF application.
* [`Migration_Tips_CoreWCF.md`](../Migration_Tips_CoreWCF.md) for a summary of key migration considerations and benefits.

---

## Phase 1: CoreWCF Host & Service Porting

* **Goal:** Create the new .NET 8 application structure that will replace the .NET Framework WCF hosts (`TodoServiceTcpHost` and `TodoServiceHost`) and service library (`TodoServiceLibrary`).

### Step 1.1: Create New .NET 8 Projects

* **Goal:** Establish the foundational project structure for the modernized application in .NET 8.

* **Actions:**

    1.  **Created a new .NET 8 Class Library project** named `TodoService.Core`.
        * **Reasoning:** A class library is needed to hold the core service contract and implementation code, making it portable and reusable. Targeting `net8.0` (or `netstandard2.0`) ensures compatibility with the modern .NET host. This project replaces the role of `TodoServiceLibrary` in the legacy setup, specifically holding the service code that will run on .NET 8.

    2.  **Created a new .NET 8 project** named `TodoApp.CoreHost`.
        * **Project Type:** Selected the **ASP.NET Core Web API** template.
        * **Target Framework:** `net8.0`.
        * **Reasoning:** This project will serve as the CoreWCF application host. The ASP.NET Core Web API template is a common and suitable starting point because it provides the necessary infrastructure (the .NET Generic Host, Kestrel web server) out-of-the-box, which CoreWCF leverages for hosting both HTTP and NetTcp endpoints. This single host project (`TodoApp.CoreHost`) will replace the two separate .NET Framework host projects (`TodoServiceTcpHost` and `TodoServiceHost`).

* **Result:** The solution now includes two new .NET 8 projects ready to receive the ported code and CoreWCF configuration.

---

### Step 1.2: Port Service Code & Contracts

* **Goal:** Move the core service logic and contract definitions from the legacy .NET Framework library (`TodoServiceLibrary`) to the new .NET 8 library (`TodoService.Core`) and prepare them for CoreWCF.
* **Actions:**

    1.  **Copied the core service code files** from `TodoServiceLibrary` to `TodoService.Core`:
        * `ITodoService.cs` (Service Contract)
        * `TodoItem.cs` (Data Contract)
        * `TodoService.cs` (Service Implementation)
        * `DeprecatedDependencyInjectionDemo.cs` (Dependency Demonstration)
    2.  **Removed .NET Framework WCF-specific attributes** from `TodoService.cs` in `TodoService.Core`.
        * Specifically, removed the `[ServiceBehavior(InstanceContextMode = Single)]` attribute from the `TodoService` class definition.
        * **Reasoning:** This attribute is specific to the .NET Framework WCF hosting runtime and how it manages service instances (PerCall, PerSession, Single). In CoreWCF, service instance lifetime is controlled by the built-in Dependency Injection container's registration (Transient, Scoped, Singleton), not by this attribute.
        * **Note on Attributes:** Attributes like `[ServiceContract]`, `[OperationContract]`, `[DataContract]`, and `[DataMember]` **are supported and required** by CoreWCF. These attributes define the *wire format* and *contract* of the service, which CoreWCF needs to understand messages and expose endpoints. They are found in the `CoreWCF.Primitives` NuGet package. Attributes tied to the old .NET Framework WCF runtime behavior or hosting model are generally not supported.
    3.  **Added the `CoreWCF.Primitives` NuGet package** to the `TodoService.Core` project.
        * **Reasoning:** This package provides the essential WCF contract attributes (`[ServiceContract]`, `[OperationContract]`, `[DataContract]`, etc.) that are needed for the service code to compile and for CoreWCF to understand the service definition.

* **Result:** The core service logic and contracts are now in the .NET 8 library, adapted by removing the .NET Framework hosting-specific attribute, and referencing the necessary CoreWCF contracts package.

---

### Step 1.3: Implement CoreWCF Basic Hosting & Bindings

* **Goal:** Configure the new .NET 8 host project (`TodoApp.CoreHost`) to serve the ported service via CoreWCF endpoints for both HTTP and NetTcp, replacing the functionality of `TodoServiceTcpHost` and `TodoServiceHost`.
* **Actions:**
    * (This section will be filled in during Step 1.3, detailing the CoreWCF NuGet packages added to the host, the configuration in `Program.cs`, and the endpoint definitions).

---

### Step 1.4: Integrate .NET 8 Built-in DI

* **Goal:** Configure the host's built-in DI container to manage the service instance and its dependencies, replacing the manual DI setup and factory in the legacy hosts.
* **Actions:**
    * (This section will be filled in during Step 1.4, detailing the DI registrations in `Program.cs`, explaining how this replaces manual DI and factories).

---

## Phase 2: Client Update & Testing

* **Goal:** Update the existing .NET Framework client to communicate with the new CoreWCF service and verify functionality.

### Step 2.1: Update Existing .NET Framework Client

* **Goal:** Point the .NET Framework client to the new CoreWCF endpoints and update its configuration.
* **Actions:**
    * (This section will be filled in during Step 2.1, detailing updating the service reference and the client's `App.config`).

---

### Step 2.2: Comprehensive Testing of Migrated Solution

* **Goal:** Verify that the client can successfully call the CoreWCF service over both endpoints.
* **Actions:**
    * (This section will be filled in during Step 2.2, outlining the testing procedures and expected results).

---

## Phase 3: Documentation & Refinement

* **Goal:** Consolidate all documentation and finalize the project.

### Step 3.1: Consolidate & Refine Documentation

* **Goal:** Organize all documentation files.
* **Actions:**
    * (This section will describe the final organization of documentation files).

---

### Step 3.2: Add Explanations of Key Differences/Benefits

* **Goal:** Clearly articulate how CoreWCF and modern .NET address the challenges seen in the legacy setup.
* **Actions:**
    * (This section will detail the comparison points).

---

### Step 3.3: Final Code Review and Cleanup

* **Goal:** Ensure the final code is clean and documentation is accurate.
* **Actions:**
    * (This section will outline final review steps).

---