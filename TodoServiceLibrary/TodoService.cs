using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoServiceLibrary
{
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
        //
        // In a real WCF application without a built-in container,
        // something *external* to this class (like a custom
        // IServiceHostFactory or an IInstanceProvider, or a
        // third-party DI container's WCF integration) would be
        // responsible for creating the DeprecatedDependencyInjectionDemo
        // instance and providing it when a TodoService instance is needed.
        public TodoService(DeprecatedDependencyInjectionDemo demo)
        {
            // Good practice to check for null dependencies
            _demo = demo ?? throw new ArgumentNullException(nameof(demo));
            Console.WriteLine("TodoService created with injected dependency.");
        }

        private static List<TodoItem> _todoItems = new List<TodoItem>()
            {
                new TodoItem { Id = 1, Title = "Learn WCF", IsCompleted = true },
                new TodoItem { Id = 2, Title = "Build Todo App", IsCompleted = false }
            };

        public TodoItem GetTodoItem(int id)
        {
            _demo.DoSomethingWithData();
            return _todoItems.FirstOrDefault(item => item.Id == id);
        }

        public List<TodoItem> GetAllTodoItems()
        {
            _demo.DoSomethingWithData();
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
}