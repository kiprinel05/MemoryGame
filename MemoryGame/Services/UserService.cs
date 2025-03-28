using System.Collections.Generic;
using System.IO;
using MemoryGame.Models;
using Newtonsoft.Json;

namespace MemoryGame.Services
{
    public class UserService
    {
        private static string filePath = "users.json";

        public static List<UserModel> LoadUsers()
        {
            if (!File.Exists(filePath))
                return new List<UserModel>();

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserModel>>(json) ?? new List<UserModel>();
        }

        public static void SaveUsers(List<UserModel> users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
