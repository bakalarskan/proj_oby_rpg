using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game.model;
using lab_game.infrastructure;
using lab_game.view;

namespace lab_game.infrastructure
{
    public sealed class MFLogger : IGameLogger
    {
        private readonly List<string> _entries = new List<string>();
        private readonly object _sync = new object();
        public string LogFilePath { get; }
        public MFLogger(string playerName, string logDirectory)
        {
            Directory.CreateDirectory(logDirectory);
            string pn = SanitizeFileName(string.IsNullOrWhiteSpace(playerName) ? "Student" : playerName.Trim());
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string suffix = Guid.NewGuid().ToString("N").Substring(0, 8);
            LogFilePath = Path.Combine(logDirectory, $"{pn}_{timestamp}_{suffix}.log");
            using FileStream fs = new FileStream(LogFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine($"Dziennik gry: {pn}");
            sw.WriteLine($"Start: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sw.WriteLine();
        }
        public void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            lock (_sync)
            {
                _entries.Add(entry);
                File.AppendAllText(LogFilePath, entry + Environment.NewLine, Encoding.UTF8);
            }
        }
        public IReadOnlyList<string> GetRecent(int n)
        {
            lock (_sync)
            {
                if (n <= 0)
                {
                    return Array.Empty<string>();
                }
                return _entries.TakeLast(n).ToList();
            }
        }
        public IReadOnlyList<string> GetAll()
        {
            lock (_sync)
            {
                return _entries.ToList();
            }
        }
        private static string SanitizeFileName(string name)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            StringBuilder sb= new StringBuilder(name.Length);
            foreach (char c in name)
            {
                sb.Append(invalid.Contains(c) ? '_' : c);
            }
            return sb.ToString();
        }
    }
}
