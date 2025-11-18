using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Command;
using TodoApp.UI.Dialogs;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        private readonly NavigationService _nav;
        private readonly TodoService _todoService;
        private readonly CategoryService _categoryService;

        private User _currentUser;
        private string _newTodoTitle;
        private string _currentCategoryName = "All Tasks";
        private Guid? _selectedCategoryId;
        private ObservableCollection<Todo> _todoList;
        private ObservableCollection<Todo> _filteredTodos;
        private ObservableCollection<Category> _categories;

        public TodoViewModel(NavigationService nav, User currentUser, TodoService todoService, CategoryService categoryService)
        {
            _nav = nav;
            _currentUser = currentUser;
            _todoService = todoService;
            _categoryService = categoryService;

            Todos = new ObservableCollection<Todo>();
            Categories = new ObservableCollection<Category>();
            FilteredTodos = new ObservableCollection<Todo>();

            InitializeCommands();

            // Check if user is not null before loading data
            if (_currentUser != null)
            {
                LoadData();
            }
            else
            {
                MessageBox.Show("Error: User not found. Please login again.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Properties
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public string NewTodoTitle
        {
            get => _newTodoTitle;
            set
            {
                _newTodoTitle = value;
                OnPropertyChanged();
            }
        }

        public string CurrentCategoryName
        {
            get => _currentCategoryName;
            set
            {
                _currentCategoryName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Todo> Todos
        {
            get => _todoList;
            set
            {
                _todoList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Todo> FilteredTodos
        {
            get => _filteredTodos;
            set
            {
                _filteredTodos = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddTodoCommand { get; private set; }
        public ICommand ToggleTodoCommand { get; private set; }
        public ICommand EditTodoCommand { get; private set; }
        public ICommand DeleteTodoCommand { get; private set; }
        public ICommand FilterByCategoryCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }
        public RelayCommand<Guid> RemoveCategoryCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private void InitializeCommands()
        {
            AddTodoCommand = new RelayCommand(o => ExecuteAddTodo(), o => CanExecuteAddTodo());
            ToggleTodoCommand = new RelayCommand(o => ExecuteToggleTodo(o as Todo));
            EditTodoCommand = new RelayCommand(o => ExecuteEditTodo(o as Todo));
            DeleteTodoCommand = new RelayCommand(o => ExecuteDeleteTodo(o as Todo));
            FilterByCategoryCommand = new RelayCommand(o => ExecuteFilterByCategory(o));
            AddCategoryCommand = new RelayCommand(o => ExecuteAddCategory());
            RemoveCategoryCommand = new RelayCommand<Guid>(ExecuteRemoveCategory); // requires RelayCommand<T>
            LogoutCommand = new RelayCommand(o => ExecuteLogout());
        }
        #endregion

        #region Command Handlers
        /*
        *  Load Data
        */
        private void LoadData() 
        {
            try
            {
                // Load data for current user
                var todos = _todoService.GetTodos(_currentUser.UserId);
                Todos.Clear();
                foreach (var todo in todos)
                {
                    // Load category name if exists
                    if (todo.CategoryId.HasValue)
                    {
                        var category = _categoryService.GetCategoryById(todo.CategoryId.Value);
                        todo.CategoryName = category?.Name;
                    }
                    Todos.Add(todo);
                }

                // Load categories for current user
                var categories = _categoryService.GetCategories(_currentUser.UserId);
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                // Show all todos initially
                FilterTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Can execute Add Todo
        */
        private bool CanExecuteAddTodo()
        {
            return !string.IsNullOrWhiteSpace(NewTodoTitle);
        }

        /*
        *  Execute Add Todo
        */
        private void ExecuteAddTodo()
        {
            try
            {
                var newTodo = new Todo()
                {
                    TodoId = Guid.NewGuid(),
                    UserId = _currentUser.UserId,
                    Title = NewTodoTitle.Trim(),
                    Description = string.Empty,
                    IsCompleted = false,
                    CategoryId = _selectedCategoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                // Load category name if a category is selected
                if (newTodo.CategoryId.HasValue)
                {
                    var category = _categoryService.GetCategoryById(newTodo.CategoryId.Value);
                    newTodo.CategoryName = category?.Name;
                }

                // Add Todo to database
                _todoService.CreateTodo(newTodo);

                Todos.Add(newTodo);
                FilterTodos();

                // Clear Input
                NewTodoTitle = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Add Category
        */
        private void ExecuteAddCategory()
        {
            try
            {
                var dialog = new AddCategoryDialog();
                if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.CategoryName))
                {   
                    var newCategory = new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        UserId = _currentUser.UserId,
                        Name = dialog.CategoryName.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    _categoryService.CreateCategory(newCategory);
                    Categories.Add(newCategory);

                    // Show all todos initially
                    FilterTodos();
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Database Error Details:");
                sb.AppendLine(ex.Message);

                if (ex.InnerException != null)
                {
                    sb.AppendLine($"\nInner Exception: {ex.InnerException.Message}");

                    if (ex.InnerException.InnerException != null)
                    {
                        sb.AppendLine($"\nDatabase Error: {ex.InnerException.InnerException.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine(sb.ToString());
                MessageBox.Show(sb.ToString(), "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Toggle Todo
        */
        private void ExecuteToggleTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                todo.UpdatedAt = DateTime.Now;

                _todoService.UpdateTodo(todo);

                OnPropertyChanged(nameof(FilteredTodos));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating todo: {ex.Message}");
            }
        }

        /*
        *  Execute Edit Todo
        */
        private void ExecuteEditTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                // Reload categories from database to ensure we have the latest
                var categories = _categoryService.GetCategories(_currentUser.UserId);

                var editDialog = new EditTodoDialog(todo, categories.ToList());
                if (editDialog.ShowDialog() == true)
                {
                    var editedTodo = editDialog.EditedTodo;

                    // Load category name if exists
                    if (editedTodo.CategoryId.HasValue)
                    {
                        var category = categories.FirstOrDefault(c => c.CategoryId == editedTodo.CategoryId.Value);
                        editedTodo.CategoryName = category?.Name;
                    }
                    else
                    {
                        editedTodo.CategoryName = null;
                    }

                    // Update the todo in the service
                    _todoService.UpdateTodo(editedTodo);

                    // IMPORTANT: Update the existing todo object in the collection
                    // Find the original todo in the Todos collection
                    var originalTodo = Todos.FirstOrDefault(t => t.TodoId == todo.TodoId);
                    if (originalTodo != null)
                    {
                        // Update all properties
                        originalTodo.Title = editedTodo.Title;
                        originalTodo.Description = editedTodo.Description;
                        originalTodo.CategoryId = editedTodo.CategoryId;
                        originalTodo.CategoryName = editedTodo.CategoryName;
                        originalTodo.ReminderTime = editedTodo.ReminderTime;
                        originalTodo.UpdatedAt = editedTodo.UpdatedAt;
                        originalTodo.IsCompleted = editedTodo.IsCompleted;
                    }

                    FilterTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Delete Todo
        */
        private void ExecuteDeleteTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{todo.Title}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _todoService.DeleteTodo(todo);
                    Todos.Remove(todo);
                    FilterTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Remove Category
        */
        private void ExecuteRemoveCategory(Guid categoryId)
        {
            var category = Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null) return;

            // Verify this category belongs to the current user
            if (category.UserId != _currentUser.UserId)
            {
                MessageBox.Show("You don't have permission to delete this category.", "Access Denied",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // IMPORTANT: Check ALL todos belonging to THIS USER that have this category
            var affectedTodos = Todos
                .Where(t => t.UserId == _currentUser.UserId &&
                            t.CategoryId.HasValue &&
                            t.CategoryId.Value == categoryId)
                .ToList();

            // Show appropriate confirmation message
            MessageBoxResult result;

            if (affectedTodos.Any())
            {
                result = MessageBox.Show(
                    $"Category '{category.Name}' has {affectedTodos.Count} todo(s) assigned to it.\n\n" +
                    "Click YES to delete the category and remove it from all todos.\n" +
                    "Click NO to cancel.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
            }
            else
            {
                result = MessageBox.Show(
                    $"Delete category '{category.Name}'?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
            }

            // If user clicked No, exit
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // Step 1: Update each affected todo to remove the category reference
                // CRITICAL: Do this in a loop with proper error handling
                int updatedCount = 0;
                foreach (var todo in affectedTodos)
                {
                    try
                    {
                        // Update the in-memory object
                        todo.CategoryId = null;
                        todo.CategoryName = null;
                        todo.UpdatedAt = DateTime.Now;

                        // Save to database immediately
                        _todoService.UpdateTodo(todo);
                        updatedCount++;

                        System.Diagnostics.Debug.WriteLine($"Updated todo: {todo.Title}, CategoryId is now: {todo.CategoryId}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to update todo {todo.Title}: {ex.Message}");
                        throw new Exception($"Failed to update todo '{todo.Title}': {ex.Message}", ex);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Successfully updated {updatedCount} todos");

                // Step 2: Force a small delay to ensure database commits are complete
                // This is a workaround for potential Entity Framework tracking issues
                System.Threading.Thread.Sleep(100);

                // Step 3: Now try to delete the category
                try
                {
                    _categoryService.DeleteCategory(category);
                    System.Diagnostics.Debug.WriteLine($"Successfully deleted category: {category.Name}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to delete category: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");

                    // If deletion fails, it might be due to lingering foreign key references
                    MessageBox.Show(
                        $"Failed to delete category from database.\n\n" +
                        $"Error: {ex.Message}\n\n" +
                        $"Details: {ex.InnerException?.Message}\n\n" +
                        $"The todos have been updated. Please try deleting the category again.",
                        "Database Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    // Reload to resync
                    LoadData();
                    return;
                }

                // Step 4: Remove from UI collection only after successful database deletion
                Categories.Remove(category);

                // Step 5: Refresh the filtered todos
                FilterTodos();

                // Show success message
                string successMsg = affectedTodos.Any()
                    ? $"Category deleted successfully.\n{affectedTodos.Count} todo(s) updated."
                    : "Category deleted successfully.";

                MessageBox.Show(successMsg, "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ExecuteRemoveCategory: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                MessageBox.Show(
                    $"Error deleting category:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                // Reload data to resync with database
                LoadData();
            }
        }

        /*
        *  Execute Filter By Category
        */
        private void ExecuteFilterByCategory(object parameter)
        {
            if (parameter is string && parameter.ToString() == "All")
            {
                _selectedCategoryId = null;
                CurrentCategoryName = "All Tasks";
            }
            else if (parameter is Guid categoryId)
            {
                _selectedCategoryId = categoryId;
                var category = Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                CurrentCategoryName = category?.Name ?? "Unknown Category";
            }

            FilterTodos();
        }

        /*
        *  Filter Todos
        */
        private void FilterTodos()
        {
            FilteredTodos.Clear();

            var filtered = _selectedCategoryId.HasValue
                ? Todos.Where(t => t.CategoryId == _selectedCategoryId.Value)
                : Todos;

            // Order by: incomplete first, then by creation date (newest first)
            foreach (var todo in filtered.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt))
            {
                FilteredTodos.Add(todo);
            }
        }

        /*
        *  Logout
        */
        private void ExecuteLogout()
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var db = new TodoDbContext();
                var userRepo = new UserRepository(db);
                _nav.NavigateTo(new LoginViewModel(_nav, new UserService(userRepo)));
            }
        }
        #endregion
    }
}
