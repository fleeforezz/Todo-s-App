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
        private readonly List<Tag> _tags;

        public EditTodoDialog()
        {
            InitializeComponent();
        }

        public EditTodoDialog(Todo todo, List<Tag> tags)
        {
            InitializeComponent();

            _orginalTodo = todo;
            _tags = tags;

            LoadData();
        }

        private void LoadData()
        {
            // Set form values
            TitleTextBox.Text = _orginalTodo.Title;
            DescriptionTextBox.Text = _orginalTodo.Description ?? string.Empty;

            // Load categories with "None" option
            var tagList = new List<CategoryOption>
            {
                new CategoryOption{ TagId = null, TagName = "None" }
            };

            tagList.AddRange(_tags.Select(c => new CategoryOption
            {
                TagId = c.TagId,
                TagName = c.TagName,
            }));

            // Set the ItemsSource directly instead of relying on binding
            CategoryComboBox.ItemsSource = tagList;

            // Select current category
            if (_orginalTodo.TagId.HasValue)
            {
                CategoryComboBox.SelectedValue = _orginalTodo.TagId.Value;
            }
            else
            {
                CategoryComboBox.SelectedIndex = 0; // Select "None"
            }

            // Set reminder time if exists
            if (_orginalTodo.DueDate.HasValue)
            {
                ReminderDatePicker.SelectedDate = _orginalTodo.DueDate.Value;
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
                TagId = (CategoryComboBox.SelectedItem as CategoryOption)?.TagId,
                DueDate = ReminderDatePicker.SelectedDate,
                CreatedAt = _orginalTodo.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            // Set category name for display
            if (EditedTodo.TagId.HasValue)
            {
                var tag = _tags.FirstOrDefault(c => c.TagId == EditedTodo.TagId);
                EditedTodo.TagName = tag?.TagName;
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
            public Guid? TagId { get; set; }
            public string TagName { get; set; }
        }
    }
}
