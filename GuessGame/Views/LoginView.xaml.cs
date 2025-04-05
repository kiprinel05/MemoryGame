using GuessGame.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GuessGame.Services;
using GuessGame.Models;
namespace GuessGame.Views
{
    public partial class LoginWindow : Window
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        private List<string> avatars = new List<string>
        {
            "Resources/Avatars/lion.png",
            "Resources/Avatars/boy.png",
            "Resources/Avatars/boy2.png"
        };

        private int currentAvatarIndex = 0;
        private bool isEditing = false;
        private User editingUser = null;

        public LoginWindow()
        {
            InitializeComponent();
            UserListBox.ItemsSource = Users;
            Users = UserDataService.LoadUsers();
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            ShowUserForm(false, null);
            UserDataService.SaveUsers(Users);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selected)
                ShowUserForm(true, selected);
            UserDataService.SaveUsers(Users);
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selected)
                Users.Remove(selected);
            UserDataService.SaveUsers(Users);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selected)
                MessageBox.Show($"Logged in as {selected.Name} with avatar {selected.AvatarPath}");
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            if (isEditing && editingUser != null)
            {
                editingUser.Name = NameTextBox.Text;
                editingUser.AvatarPath = avatars[currentAvatarIndex];
                UserListBox.Items.Refresh();
            }
            else
            {
                var newUser = new User
                {
                    Name = NameTextBox.Text,
                    AvatarPath = avatars[currentAvatarIndex]
                };
                Users.Add(newUser);
            }

            HideUserForm();
        }

        private void CancelUser_Click(object sender, RoutedEventArgs e)
        {
            HideUserForm();
        }

        private void PrevAvatar_Click(object sender, RoutedEventArgs e)
        {
            currentAvatarIndex = (currentAvatarIndex - 1 + avatars.Count) % avatars.Count;
            UpdateAvatarImage();
        }

        private void NextAvatar_Click(object sender, RoutedEventArgs e)
        {
            currentAvatarIndex = (currentAvatarIndex + 1) % avatars.Count;
            UpdateAvatarImage();
        }

        private void UpdateAvatarImage()
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{avatars[currentAvatarIndex]}", UriKind.Absolute);
                AvatarImage.Source = new BitmapImage(uri);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading avatar: {ex.Message}");
            }
        }


        private void ShowUserForm(bool edit, User user)
        {
            UserFormPanel.Visibility = Visibility.Visible;
            isEditing = edit;
            editingUser = user;

            if (edit && user != null)
            {
                NameTextBox.Text = user.Name;
                currentAvatarIndex = avatars.IndexOf(user.AvatarPath);
            }
            else
            {
                NameTextBox.Text = "";
                currentAvatarIndex = 0;
            }

            UpdateAvatarImage();
        }

        private void HideUserForm()
        {
            UserFormPanel.Visibility = Visibility.Collapsed;
            NameTextBox.Text = "";
        }

        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // poți activa/dezactiva butoane aici dacă vrei
        }
    }


}
