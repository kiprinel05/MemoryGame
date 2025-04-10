using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GuessGame.Models;
using System.Text.Json;
using GuessGame.ViewModels;

namespace GuessGame.Views
{
    public partial class GameWindow : Window
    {
        private GameViewModel _viewModel;

        public GameWindow(User user)    
        {
            InitializeComponent();
            _viewModel = new GameViewModel(user);
            DrawBoard();
            _viewModel.TimeUpdated += UpdateTimerDisplay;
            _viewModel.GameLost += OnGameLost;
            _viewModel.GameWon += OnGameWon;
        }

        private void DrawBoard()
        {
            GameBoard.Children.Clear();
            GameBoard.Rows = _viewModel.Rows;
            GameBoard.Columns = _viewModel.Columns;

            for (int i = 0; i < _viewModel.TileImages.Count; i++)
            {
                var btn = new Button
                {
                    Tag = i,
                    Background = Brushes.Gray,
                    Margin = new Thickness(4),
                    Content = null,
                    Width = 100,
                    Height = 100
                };

                if (_viewModel.RevealedTiles[i])
                    ShowImage(btn, _viewModel.TileImages[i]);

                btn.Click += Tile_Click;
                GameBoard.Children.Add(btn);
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || _viewModel.SecondClicked != null) return;

            int index = (int)btn.Tag;
            if (_viewModel.RevealedTiles[index]) return;

            ShowImage(btn, _viewModel.TileImages[index]);

            if (_viewModel.FirstClicked == null)
            {
                _viewModel.FirstClicked = btn;
            }
            else
            {
                _viewModel.SecondClicked = btn;
                int firstIndex = (int)_viewModel.FirstClicked.Tag;
                int secondIndex = (int)_viewModel.SecondClicked.Tag;

                if (_viewModel.TileImages[firstIndex] == _viewModel.TileImages[secondIndex] && firstIndex != secondIndex)
                {
                    _viewModel.RevealedTiles[firstIndex] = true;
                    _viewModel.RevealedTiles[secondIndex] = true;
                    _viewModel.FirstClicked = _viewModel.SecondClicked = null;
                    _viewModel.CheckWinCondition();
                    _viewModel.SaveGame();
                }
                else
                {
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            _viewModel.FirstClicked.Content = null;
                            _viewModel.SecondClicked.Content = null;
                            _viewModel.FirstClicked = _viewModel.SecondClicked = null;
                        });
                    });
                }
            }
        }

        private void ShowImage(Button btn, string imageName)
        {
            string uri = $"pack://application:,,,/Resources/GameImages/{_viewModel.SelectedCategory}/{imageName}";

            var img = new Image
            {
                Source = new BitmapImage(new Uri(uri)),
                Stretch = Stretch.Uniform,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            btn.Content = img;
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            var timeDialog = new TimeLimitDialog { Owner = this };
            if (timeDialog.ShowDialog() == true)
            {
                _viewModel.InitializeTimer(timeDialog.SelectedTime);
                _viewModel.NewGame();
                DrawBoard();
            }
        }


        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            var loadWindow = new LoadGameWindow(_viewModel.User.Name)
            {
                Owner = this
            };

            if (loadWindow.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(loadWindow.SelectedSavePath);
                    var data = JsonSerializer.Deserialize<GameSave>(json);

                    _viewModel.LoadFromSave(data);
                    DrawBoard();
                }
                catch {}
            }
        }
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveGame();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is string category)
            {
                _viewModel.SelectedCategory = category;
                _viewModel.NewGame();
                DrawBoard();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Standard_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetToStandardSize();
            DrawBoard();
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            var sizeDialog = new CustomSizeDialog { Owner = this };
            if (sizeDialog.ShowDialog() == true)
            {
                var timeDialog = new TimeLimitDialog { Owner = this };
                if (timeDialog.ShowDialog() == true)
                {
                    _viewModel.InitializeTimer(timeDialog.SelectedTime);
                    _viewModel.SetBoardSize(sizeDialog.Rows, sizeDialog.Columns);
                    DrawBoard();
                }
            }
        }
        private void UpdateTimerDisplay()
        {
            Dispatcher.Invoke(() =>
            {
                TimerText.Text = $"Time Remaining: {_viewModel.RemainingTime:mm\\:ss}";

                if (_viewModel.RemainingTime.TotalSeconds < 30)
                {
                    TimerText.Foreground = Brushes.Red;
                }
                else if (_viewModel.RemainingTime.TotalSeconds < 60)
                {
                    TimerText.Foreground = Brushes.Yellow;
                }
                else
                {
                    TimerText.Foreground = Brushes.White;
                }
            });
        }

        private void OnGameLost()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("You Lost", "Game Lost", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Close();
            });
        }
        private void OnGameWon()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("You won!", "Game Won", MessageBoxButton.OK, MessageBoxImage.Information);

                _viewModel.SaveStatistics(true);

                Close();
            });
        }
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow();
            statisticsWindow.Owner = this;
            statisticsWindow.ShowDialog();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new About();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }

}