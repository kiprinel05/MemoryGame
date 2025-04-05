using GuessGame.Models;
using System;
using System.IO;
using System.Text.Json;

namespace GuessGame.Services
{
    public static class GameSaveService
    {
        private static string GetSavePath(string userName) =>
            Path.Combine("saves", $"{userName}_save.json");

        public static GameSave? Load(string userName)
        {
            try
            {
                var path = GetSavePath(userName);
                if (!File.Exists(path)) return null;

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<GameSave>(json);
            }
            catch
            {
                return null;
            }
        }

        public static void Save(GameSave save)
        {
            try
            {
                Directory.CreateDirectory("saves");
                var json = JsonSerializer.Serialize(save, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(GetSavePath(save.UserName), json);
            }
            catch
            {
                // log eventual
            }
        }

        public static void Delete(string userName)
        {
            try
            {
                var path = GetSavePath(userName);
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // log eventual
            }
        }
    }
}
