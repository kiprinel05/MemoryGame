using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GuessGame.Models;
using GuessGame.Views;

namespace GuessGame.ViewModels
{
    public class GameViewModel
    {
        public User User { get; private set; }
        public string SelectedCategory { get; set; } = "Flowers";
        public int Rows { get; set; } = 4;
        public int Columns { get; set; } = 4;

        public List<string> TileImages { get; private set; }
        public List<bool> RevealedTiles { get; private set; }
        public Button FirstClicked { get; set; }
        public Button SecondClicked { get; set; }
        public string SaveDirectory => Path.Combine("SavedGames", User.Name);
        public string SaveFilePath => Path.Combine("SavedGames", $"{User.Name}.json");


        public TimeSpan TimeLimit { get; private set; }
        public TimeSpan RemainingTime { get; private set; }
        private DateTime _gameStartTime;
        private DispatcherTimer _gameTimer;
        public event Action TimeUpdated;
        public event Action GameLost;


        public GameViewModel(User user)
        {
            User = user;
            try
            {
                if (File.Exists(SaveFilePath))
                    LoadGame();
                else
                    NewGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}");
                NewGame();
            }
        }

        public void NewGame()
        {
            GenerateTiles();
        }

        public void SetBoardSize(int rows, int columns)
        {
            // Validăm că avem un număr par de celule
            if ((rows * columns) % 2 != 0)
            {
                MessageBox.Show("Total tiles must be even (Rows × Columns should be even)");
                return;
            }

            if (rows < 2 || rows > 6 || columns < 2 || columns > 6)
            {
                MessageBox.Show("Rows and columns must be between 2 and 6");
                return;
            }

            Rows = rows;
            Columns = columns;
            NewGame();
        }

        public void ResetToStandardSize()
        {
            Rows = 4;
            Columns = 4;
            NewGame();
        }
        private void GenerateTiles()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "GameImages", SelectedCategory);
            var files = Directory.GetFiles(basePath, "*.png")
                                 .Select(Path.GetFileName)
                                 .ToList();

            int totalTiles = Rows * Columns;
            int neededPairs = totalTiles / 2;

            if (files.Count < neededPairs)
            {
                MessageBox.Show("Nu sunt suficiente imagini în categoria selectată.");
                return;
            }

            var selected = files.Take(neededPairs).ToList();
            var all = selected.Concat(selected).OrderBy(_ => Guid.NewGuid()).ToList();

            TileImages = all;
            RevealedTiles = Enumerable.Repeat(false, totalTiles).ToList();
        }

        public void SaveGame()
        {
            var data = new GameSave
            {
                UserName = User.Name,
                Category = SelectedCategory,
                Rows = Rows,
                Columns = Columns,
                TileImages = TileImages,
                RevealedTiles = RevealedTiles,
                TimeLimit = TimeLimit,
                ElapsedTime = DateTime.Now - _gameStartTime,
                SaveTime = DateTime.Now
            };

            Directory.CreateDirectory(SaveDirectory);
            string fileName = $"{SelectedCategory}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string path = Path.Combine(SaveDirectory, fileName);
            File.WriteAllText(path, JsonSerializer.Serialize(data));
        }
        public void LoadGame()
        {
            try
            {
                var json = File.ReadAllText(SaveFilePath);
                var data = JsonSerializer.Deserialize<GameSave>(json);

                SelectedCategory = data.Category;
                Rows = data.Rows;
                Columns = data.Columns;
                TileImages = data.TileImages;
                RevealedTiles = data.RevealedTiles;
            }
            catch
            {
                MessageBox.Show("Eroare la incarcarea jocului");
                NewGame();
            }
        }

        public void LoadFromSave(GameSave data)
        {
            SelectedCategory = data.Category;
            Rows = data.Rows;
            Columns = data.Columns;
            TileImages = data.TileImages;
            RevealedTiles = data.RevealedTiles;
            TimeLimit = data.TimeLimit;

            var timeSinceSave = DateTime.Now - data.SaveTime;
            RemainingTime = data.TimeLimit - data.ElapsedTime - timeSinceSave;

            if (RemainingTime > TimeSpan.Zero)
            {
                _gameStartTime = DateTime.Now - data.ElapsedTime;
                InitializeTimer(TimeLimit);
            }
            else
            {
                GameLost?.Invoke();
            }
        }

        public void InitializeTimer(TimeSpan timeLimit)
        {
            TimeLimit = timeLimit;
            RemainingTime = timeLimit;
            _gameStartTime = DateTime.Now;

            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _gameStartTime;
            RemainingTime = TimeLimit - elapsed;

            TimeUpdated?.Invoke();

            if (RemainingTime <= TimeSpan.Zero)
            {
                _gameTimer.Stop();
                GameLost?.Invoke();
            }
        }
    }
}