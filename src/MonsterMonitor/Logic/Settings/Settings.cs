using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace MonsterMonitor.Logic.Settings
{
    public class Settings
    {
        public string SystemPassword { get; set; }
        public string Proxy { get; set; }
        public string SshHost { get; set; }
        public bool PingCheck { get; set; }
        public string ThreeProxyPassword { get; set; }

        public static Settings Load()
        {
            var configDir = Application.StartupPath.Replace(Application.ProductVersion, "");
            var settingsPath = Path.Combine(configDir, _settingsFileName);
            if (!File.Exists(settingsPath))
            {
                return new Settings
                {
                    ThreeProxyPassword = CreatePassword(10)
                };
            }

            var settingsText = File.ReadAllText(settingsPath);

            Settings settings;

            try
            {
                settings = JsonConvert.DeserializeObject<Settings>(settingsText);
                if (settings == null)
                {
                    return null;
                }
                var password = new PasswordEncryptionClass().Decrypt("user", settings.SystemPassword, "http://sibur.ru");
                settings.SystemPassword = password;
            }
            catch (JsonReaderException)
            {
                return null;
            }

            return settings;
        }

        public void Save()
        {
            var password = new PasswordEncryptionClass().Encrypt("user", this.SystemPassword, "http://sibur.ru");
            this.SystemPassword = password;

            var configDir = Application.StartupPath.Replace(Application.ProductVersion, "");
            var settingsPath = Path.Combine(configDir, _settingsFileName);
            var settingsString = JsonConvert.SerializeObject(this);
            File.WriteAllText(settingsPath, settingsString);
        }

        private static readonly string _settingsFileName = "settings.json";

        private static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
