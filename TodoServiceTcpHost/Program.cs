using System;
using System.ServiceModel; // Required for ServiceHost
using System.ServiceModel.Description; // Required for ServiceMetadataBehavior (if configured in code)
using TodoServiceLibrary; // Required to reference your service implementation

namespace TodoServiceTcpHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // --- WCF Host Implementation ---

            // Base Address for the NetTcp endpoint
            // This uses the net.tcp URI scheme.
            // Choose a port that isn't already in use (e.g., 8080).
            // The path "TodoService" is just a convention.
            Uri baseAddress = new Uri("net.tcp://localhost:8080/TodoService");

            // Create the ServiceHost instance within a using block
            // The ServiceHost manages the lifetime of the service and its endpoints.
            // It loads configuration from App.config by default if specified.
            using (ServiceHost host = new ServiceHost(typeof(TodoService), baseAddress))
            {
                try
                {
                    // Optional: If you didn't use configuration, you could add endpoints in code like this:
                    // host.AddServiceEndpoint(typeof(ITodoService), new NetTcpBinding(), "");
                    // host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

                    // Open the ServiceHost
                    // This makes the service endpoint(s) available for clients to connect.
                    host.Open();

                    Console.WriteLine($"The Todo Service is ready at {baseAddress}");
                    Console.WriteLine("Press <ENTER> to stop the service.");
                    Console.ReadLine();

                    // Close the ServiceHost when the user presses Enter
                    host.Close();
                    Console.WriteLine("The Todo Service is closed.");
                }
                catch (Exception ex)
                {
                    // Handle exceptions during hosting
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // If the host faulted, abort it
                    if (host.State != CommunicationState.Closed)
                    {
                        host.Abort();
                    }
                }
            }

            // --- Migration Challenge Hint: Hosting ---
            // In CoreWCF (.NET 8), you won't use System.ServiceModel.ServiceHost.
            // Instead, CoreWCF services are hosted within the standard .NET Generic Host
            // or ASP.NET Core Host. You'll configure CoreWCF endpoints and behaviors
            // using code in your Program.cs or Startup.cs (depending on the host template),
            // integrating with the host's dependency injection and middleware pipeline.
            // This is a fundamental shift from the .NET Framework App.config/ServiceHost model.
        }
    }
}