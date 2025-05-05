using Microsoft.EntityFrameworkCore;

namespace TodoApp.Core
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> TodoItems => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasKey(t => t.TodoId);
        }
    }

    public class DeprecatedEntityFrameworkDependency
    {
        private readonly TodoContext _context;

        public DeprecatedEntityFrameworkDependency(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async Task EnsureDatabaseExistsAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task<List<Todo>> GetTodoItemsFromDatabaseAsync()
        {
            await EnsureDatabaseExistsAsync();

            try
            {
                return await _context.TodoItems.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTodoItemsFromDatabase: {ex.Message}");
                return new List<Todo>();
            }
        }

        public async Task AddTodoItemToDatabaseAsync(string title)
        {
            await EnsureDatabaseExistsAsync();

            try
            {
                await _context.TodoItems.AddAsync(new Todo
                {
                    Title = title,
                    IsCompleted = false
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddTodoItemToDatabase: {ex.Message}");
            }
        }

        public async Task DemonstrateEntityFrameworkAsync()
        {
            Console.WriteLine("Demonstrating Entity Framework Core usage...");

            await AddTodoItemToDatabaseAsync("Example Todo from Migrated Code");
            var todoItems = await GetTodoItemsFromDatabaseAsync();

            if (todoItems.Any())
            {
                foreach (var todo in todoItems)
                {
                    Console.WriteLine($"  - {todo.TodoId}: {todo.Title} (Completed: {todo.IsCompleted})");
                }
            }
            else
            {
                Console.WriteLine("  No Todo items found.");
            }
        }
    }
}
