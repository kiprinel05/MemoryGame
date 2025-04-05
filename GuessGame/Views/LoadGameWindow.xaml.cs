using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GuessGame.Views
{
    public partial class LoadGameWindow : Window
    {
        public string SelectedSavePath { get; private set; }
        private readonly string _userName;
        public LoadGameWindow(string userName)
        {
            InitializeComponent();
            _userName = userName;
            LoadSaveFiles(_userName);
        }

        private void LoadSaveFiles(string userName)
        {
            string userDir = Path.Combine("SavedGames", userName);
            if (!Directory.Exists(userDir))
            {
                MessageBox.Show("Nu sunt salvari");
                Close();
                return;
            }

            var files = Directory.GetFiles(userDir, "*.json");
            foreach (var file in files)
            {
                SavesListBox.Items.Add(Path.GetFileName(file));
            }
        }

        private void SavesListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SavesListBox.SelectedItem is string fileName)
            {
                string userDir = Path.Combine("SavedGames", _userName);
                SelectedSavePath = Path.Combine(userDir, fileName);
                DialogResult = true;
                Close();
            }
        }
        private class SaveItem
        {
            public string FileName { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
