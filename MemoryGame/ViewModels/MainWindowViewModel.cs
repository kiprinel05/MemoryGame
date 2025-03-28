using CommunityToolkit.Mvvm.ComponentModel;

namespace MemoryGame.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public string Title { get; set; } = "Memory Game - WPF";
    }
}
