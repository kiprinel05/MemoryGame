using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using GuessGame.Models;

namespace GuessGame.Services
{
    public static class UserDataService
    {
        private static readonly string FilePath = "users.json";

        public static ObservableCollection<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new ObservableCollection<User>();

                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<ObservableCollection<User>>(json) ?? new ObservableCollection<User>();
            }
            catch
            {
                return new ObservableCollection<User>();
            }
        }

        public static void SaveUsers(ObservableCollection<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch
            {
            }
        }
    }
}
