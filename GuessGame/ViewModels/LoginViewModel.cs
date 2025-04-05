using GuessGame.Models;
using GuessGame.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace GuessGame.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly List<string> avatars = new()
        {
            "pack://application:,,,/Resources/Avatars/av1.jpg",
            "pack://application:,,,/Resources/Avatars/av2.jpg",
            "pack://application:,,,/Resources/Avatars/av3.jpg",
            "pack://application:,,,/Resources/Avatars/av4.jpg",
            "pack://application:,,,/Resources/Avatars/av5.jpg"
        };

        private int _currentAvatarIndex = 0;
        public string CurrentAvatar => avatars[_currentAvatarIndex];

        private Visibility _userFormVisibility = Visibility.Collapsed;
        public Visibility UserFormVisibility
        {
            get => _userFormVisibility;
            set
            {
                _userFormVisibility = value;
                OnPropertyChanged();
            }
        }

        private User _editingUser;
        public User EditingUser
        {
            get => _editingUser;
            set
            {
                _editingUser = value;
                OnPropertyChanged();
            }
        }

        private User _originalUserForEdit; // 🔧 păstrează userul original când edităm

        // Commands
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand NextAvatarCommand { get; }
        public ICommand PrevAvatarCommand { get; }
        public ICommand SaveUserCommand => new RelayCommand(SaveUser);
        public ICommand CancelEditCommand => new RelayCommand(CancelEdit);

        public LoginViewModel()
        {
            LoadUsers();

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser, () => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);
            PlayCommand = new RelayCommand(Play, () => SelectedUser != null);
            NextAvatarCommand = new RelayCommand(NextAvatar);
            PrevAvatarCommand = new RelayCommand(PrevAvatar);
        }

        private void LoadUsers()
        {
            var loaded = UserDataService.LoadUsers();
            foreach (var user in loaded)
                Users.Add(user);
        }

        private void SaveUsers()
        {
            UserDataService.SaveUsers(Users);
        }

        private void AddUser()
        {
            SelectedUser = null;
            OnPropertyChanged(nameof(SelectedUser));

            EditingUser = new User();
            _currentAvatarIndex = 0;
            _originalUserForEdit = null;
            UserFormVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(CurrentAvatar));
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;

            _originalUserForEdit = SelectedUser;

            EditingUser = new User
            {
                Name = SelectedUser.Name,
                AvatarPath = SelectedUser.AvatarPath
            };

            _currentAvatarIndex = avatars.IndexOf(SelectedUser.AvatarPath);
            if (_currentAvatarIndex < 0) _currentAvatarIndex = 0;

            UserFormVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(CurrentAvatar));
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {

                Users.Remove(SelectedUser);
                SelectedUser = null;
                SaveUsers();
            }
        }

        private void SaveUser()
        {
            if (string.IsNullOrWhiteSpace(EditingUser?.Name))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            EditingUser.AvatarPath = avatars[_currentAvatarIndex];

            if (_originalUserForEdit != null)
            {
                // Editare
                _originalUserForEdit.Name = EditingUser.Name;
                _originalUserForEdit.AvatarPath = EditingUser.AvatarPath;
                SelectedUser = _originalUserForEdit;
            }
            else
            {
                // Adăugare
                var newUser = new User
                {
                    Name = EditingUser.Name,
                    AvatarPath = EditingUser.AvatarPath
                };
                Users.Add(newUser);
                SelectedUser = newUser;
            }

            SaveUsers();
            UserFormVisibility = Visibility.Collapsed;
            _originalUserForEdit = null;
        }

        private void CancelEdit()
        {
            EditingUser = null;
            _originalUserForEdit = null;
            UserFormVisibility = Visibility.Collapsed;
        }

        private void Play()
        {
            if (SelectedUser != null)
            {
                MessageBox.Show($"Logged in as {SelectedUser.Name} with avatar {SelectedUser.AvatarPath}");
            }
        }

        private void NextAvatar()
        {
            _currentAvatarIndex = (_currentAvatarIndex + 1) % avatars.Count;
            OnPropertyChanged(nameof(CurrentAvatar));
        }

        private void PrevAvatar()
        {
            _currentAvatarIndex = (_currentAvatarIndex - 1 + avatars.Count) % avatars.Count;
            OnPropertyChanged(nameof(CurrentAvatar));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
