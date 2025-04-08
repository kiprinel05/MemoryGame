using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using GuessGame.Models;

namespace GuessGame.Services
{
    public static class StatisticsService
    {
        private static readonly string FilePath = "statistics.json";

        public static List<UserStatistics> LoadStatistics()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<UserStatistics>();

                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<UserStatistics>>(json) ?? new List<UserStatistics>();
            }
            catch
            {
                return new List<UserStatistics>();
            }
        }

        public static void SaveStatistics(List<UserStatistics> statistics)
        {
            try
            {
                var json = JsonSerializer.Serialize(statistics, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch {}
        }

        public static void UpdateUserStatistics(string userName, bool won)
        {
            var statistics = LoadStatistics();
            var userStats = statistics.FirstOrDefault(s => s.UserName == userName);

            if (userStats == null)
            {
                userStats = new UserStatistics { UserName = userName };
                statistics.Add(userStats);
            }

            userStats.GamesPlayed++;
            if (won) userStats.GamesWon++;

            SaveStatistics(statistics);
        }
    }
}