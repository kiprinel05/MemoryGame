using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MemoryGame.Models;

namespace MemoryGame.Services
{
    public class GameService
    {
        private static string filePath = "saved_games.json";

        public static List<GameModel> LoadSavedGames()
        {
            if (!File.Exists(filePath))
                return new List<GameModel>();

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GameModel>>(json) ?? new List<GameModel>();
        }

        public static void SaveGame(GameModel game)
        {
            var savedGames = LoadSavedGames();

            // Suprascriem jocul dacă există deja unul pentru același utilizator
            savedGames.RemoveAll(g => g.Username == game.Username);

            savedGames.Add(game);
            var json = JsonConvert.SerializeObject(savedGames, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static GameModel LoadUserGame(string username)
        {
            var savedGames = LoadSavedGames();
            return savedGames.Find(g => g.Username == username);
        }
    }
}
