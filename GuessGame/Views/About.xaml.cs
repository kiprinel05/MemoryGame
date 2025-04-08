using System.Windows;

namespace GuessGame.Views
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OnGitHubButtonClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://github.com/kiprinel05") { UseShellExecute = true });
        }
    }
}
