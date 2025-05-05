namespace TodoApp.Core;

/*
* "TodoService" class implements the "ITodoService" interface.
* This class contains the actual logic for each service operation.
* For simplicity, an in-memory "_todoItems" is being use
* */
public class TodoService : ITodoService
{
    // This field will hold the dependency, which is now injected
    // rather than created internally.
    private readonly DeprecatedDependencyInjectionDemo _demo;

    // Constructor Injection:
    // The dependency (DeprecatedDependencyInjectionDemo) is now
    // passed into the constructor. This is the standard way
    // to apply dependency injection to a class.
    public TodoService(DeprecatedDependencyInjectionDemo demo)
    {
        _demo = demo ?? throw new ArgumentNullException(nameof(demo));
        Console.WriteLine("TodoService created with injected dependency.");
    }

    // Note: Because InstanceContextMode is Single, this constructor is
    // called only ONCE when the ServiceHost is opened.

    public string GetData(int value)
    {
        _demo.DoSomethingWithData();
        return $"You requested data for: {value}";
    }

    public void DoWork()
    {
        Console.WriteLine("TodoService.DoWork called.");
    }

    // NOTE: The _todoItems list is static, so it was already shared
    // across all service instances (even with PerCall mode).
    // With InstanceContextMode.Single, the service *instance* itself
    // is also shared. If _todoItems were NOT static but an instance field,
    // it would now be shared across all client calls, requiring thread-safety.
    private static List<TodoItem> _todoItems = new List<TodoItem>()
        {
            new TodoItem { Id = 1, Title = "Learn WCF", IsCompleted = true },
            new TodoItem { Id = 2, Title = "Build Todo App", IsCompleted = false }
        };

    public TodoItem GetTodoItem(int id)
    {
        return _todoItems.FirstOrDefault(item => item.Id == id);
    }

    public List<TodoItem> GetAllTodoItems()
    {
        return _todoItems;
    }

    public void AddTodoItem(TodoItem item)
    {
        item.Id = _todoItems.Count + 1; // Simple ID generation
        _todoItems.Add(item);
    }

    public void UpdateTodoItem(TodoItem item)
    {
        var existingItem = _todoItems.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Title = item.Title;
            existingItem.IsCompleted = item.IsCompleted;
        }
    }

    public void DeleteTodoItem(int id)
    {
        _todoItems.RemoveAll(item => item.Id == id);
    }
}