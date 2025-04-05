using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GuessGame.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _name;
        private string _avatarPath;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AvatarPath
        {
            get => _avatarPath;
            set
            {
                if (_avatarPath != value)
                {
                    _avatarPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
