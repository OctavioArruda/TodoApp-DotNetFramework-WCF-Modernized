using SoapCore;
using TodoApp.Core; // Ensure this namespace contains ITodoService and TodoService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSoapCore(); // Add SOAP support

// Register your TodoService implementation against the ITodoService interface
builder.Services.AddTransient<ITodoService, TodoService>();

// Now that we are using .NET 8, we can register the dependency on the DI container
builder.Services.AddTransient<DeprecatedDependencyInjectionDemo>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Make sure routing is enabled

app.UseAuthorization();

app.MapControllers();

// Explicitly cast 'app' to IApplicationBuilder
(app as IApplicationBuilder).UseSoapEndpoint<ITodoService>(
    "/TodoService.svc",
    new SoapEncoderOptions(),
    SoapSerializer.XmlSerializer
);

app.Run();