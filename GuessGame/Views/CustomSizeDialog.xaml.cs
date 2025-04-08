using System.Windows.Controls;
using System.Windows;

namespace GuessGame.Views
{
    public partial class CustomSizeDialog : Window
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public CustomSizeDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Rows = int.Parse(((ComboBoxItem)RowsComboBox.SelectedItem).Content.ToString());
            Columns = int.Parse(((ComboBoxItem)ColumnsComboBox.SelectedItem).Content.ToString());

            if ((Rows * Columns) % 2 != 0)
            {
                MessageBox.Show("Total tiles must be even");
                return;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}