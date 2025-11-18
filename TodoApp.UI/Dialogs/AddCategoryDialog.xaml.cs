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

namespace TodoApp.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddCategoryDialog.xaml
    /// </summary>
    public partial class AddCategoryDialog : Window
    {
        public string CategoryName { get; private set; }

        public AddCategoryDialog()
        {
            InitializeComponent();

            // Focus on textbox when dialog opens
            Loaded += (s, e) => CategoryNameTextBox.Focus();

            // Allow Enter key to submit
            CategoryNameTextBox.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    AddButton_Click(s, e);
                }
            };
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CategoryNameTextBox.Focus();
                return;
            }

            CategoryName = CategoryNameTextBox.Text.Trim();
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
    }
}
