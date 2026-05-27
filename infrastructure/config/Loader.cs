using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lab_game.infrastructure
{
    public static class Loader
    {
        public static Config Load(string path)
        {
            if (!File.Exists(path))
            {
                Config defaultConfig = new Config();
                Save(path, defaultConfig);
                return defaultConfig;
            }
            string json = File.ReadAllText(path);
            Config? config = JsonSerializer.Deserialize<Config>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (config == null)
            {
                throw new InvalidOperationException("Niepoprawna zawartość pliku konfiguracyjnego.");
            }
            return Normalize(config);
        }
        private static void Save(string path, Config config)
        {
            string? directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        private static Config Normalize(Config config)
        {
            string playerName = string.IsNullOrWhiteSpace(config.PlayerName) ? "Student" : config.PlayerName.Trim();
            string logDirectory = string.IsNullOrWhiteSpace(config.LogDirectory) ? "logs" : config.LogDirectory.Trim();
            return new Config
            {
                PlayerName = playerName,
                LogDirectory = logDirectory
            };
        }
    }
}
