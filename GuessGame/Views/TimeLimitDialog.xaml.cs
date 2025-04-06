using System.Windows.Controls;
using System.Windows;

namespace GuessGame.Views
{
    public partial class TimeLimitDialog : Window
    {
        public TimeSpan SelectedTime { get; private set; }

        public TimeLimitDialog()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((ComboBoxItem)TimeComboBox.SelectedItem).Content.ToString(), out int minutes))
            {
                SelectedTime = TimeSpan.FromMinutes(minutes);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a valid time limit.");
            }
        }
    }
}