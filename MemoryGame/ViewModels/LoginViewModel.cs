using MemoryGame.Models;
using MemoryGame.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;


namespace MemoryGame.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }
        public ICommand UploadAvatarCommand { get; }
        public ICommand PlayCommand { get; }

        public bool CanPlay => SelectedUser != null;

        public LoginViewModel()
        {
            Users = new ObservableCollection<UserModel>(UserService.LoadUsers());
            UploadAvatarCommand = new RelayCommand(UploadAvatar);
            PlayCommand = new RelayCommand(PlayGame, () => CanPlay);
        }

        private void UploadAvatar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.png, *.gif)|*.jpg;*.png;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedUser.AvatarPath = openFileDialog.FileName;
                UserService.SaveUsers(new List<UserModel>(Users));
            }
        }

        private void PlayGame()
        {
            // Navigare către GameView
        }
    }
}
