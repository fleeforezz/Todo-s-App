using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TodoApp.DAL.Entities;

namespace TodoApp.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for EditTodoDialog.xaml
    /// </summary>
    public partial class EditTodoDialog : Window
    {
        public Todo EditedTodo { get; private set; }
        private readonly Todo _orginalTodo;
        private readonly List<Category> _categories;

        public EditTodoDialog()
        {
            InitializeComponent();
        }

        public EditTodoDialog(Todo todo, List<Category> categories)
        {
            InitializeComponent();

            _orginalTodo = todo;
            _categories = categories;

            LoadData();
        }

        private void LoadData()
        {
            // Set form values
            TitleTextBox.Text = _orginalTodo.Title;
            DescriptionTextBox.Text = _orginalTodo.Description ?? string.Empty;

            // Load categories with "None" option
            var categoryList = new List<CategoryOption>
            {
                new CategoryOption{ CategoryId = null, Name = "None" }
            };

            categoryList.AddRange(_categories.Select(c => new CategoryOption
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
            }));

            // Set the ItemsSource directly instead of relying on binding
            CategoryComboBox.ItemsSource = categoryList;

            // Select current category
            if (_orginalTodo.CategoryId.HasValue)
            {
                CategoryComboBox.SelectedValue = _orginalTodo.CategoryId.Value;
            }
            else
            {
                CategoryComboBox.SelectedIndex = 0; // Select "None"
            }

            // Set reminder time if exists
            if (_orginalTodo.ReminderTime.HasValue)
            {
                ReminderDatePicker.SelectedDate = _orginalTodo.ReminderTime.Value;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Please enter a title.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBox.Focus();
                return;
            }

            // Create edited Todo
            EditedTodo = new Todo
            {
                TodoId = _orginalTodo.TodoId,
                UserId = _orginalTodo.UserId,
                Title = TitleTextBox.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                    ? null
                    : DescriptionTextBox.Text.Trim(),
                IsCompleted = _orginalTodo.IsCompleted,
                CategoryId = (CategoryComboBox.SelectedItem as CategoryOption)?.CategoryId,
                ReminderTime = ReminderDatePicker.SelectedDate,
                CreatedAt = _orginalTodo.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            // Set category name for display
            if (EditedTodo.CategoryId.HasValue)
            {
                var category = _categories.FirstOrDefault(c => c.CategoryId == EditedTodo.CategoryId);
                EditedTodo.CategoryName = category?.Name;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Helper class for ComboBox binding
        private class CategoryOption
        {
            public Guid? CategoryId { get; set; }
            public string Name { get; set; }
        }
    }
}
