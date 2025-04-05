using GuessGame.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GuessGame.ViewModels
{

    public class LoginViewModel : INotifyPropertyChanged
    {
        private User _selectedUser;

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanPlayOrDelete));
            }
        }

        public bool CanPlayOrDelete => SelectedUser != null;

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand CancelCommand { get; }


        public LoginViewModel()
        {
            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => CanPlayOrDelete);
            PlayCommand = new RelayCommand(Play, () => CanPlayOrDelete);
            EditUserCommand = new RelayCommand(EditUser, () => CanPlayOrDelete);
            CancelCommand = new RelayCommand(Cancel);

            LoadUsers();
        }

        private void AddUser()
        {
            Users.Add(new User { Name = "New User", AvatarPath = "/Resources/Avatars/lion.png" });
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
                Users.Remove(SelectedUser);
        }

        private void Play()
        {
            // Vom face asta după ce avem fereastra jocului
        }

        private void LoadUsers()
        {
            // Vom adăuga mai târziu funcția de încărcare din fișier
        }
        private void EditUser()
{
    if (SelectedUser != null)
    {
        var input = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter new name for the user:",
            "Edit User",
            SelectedUser.Name);

        if (!string.IsNullOrWhiteSpace(input))
        {
            SelectedUser.Name = input;
            OnPropertyChanged(nameof(Users)); // forțăm actualizarea
        }
    }
}

private void Cancel()
{
}


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
