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

        public GameViewModel(User user)
        {
            User = user;
            if (File.Exists(SaveFilePath))
                LoadGame();
            else
                NewGame();
        }

        public void NewGame()
        {
            GenerateTiles();
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
                RevealedTiles = RevealedTiles
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
        }

    }
}
