using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace TgDelayedShitpostBot
{
    public class Settings
    {
        private Settings() { }

        private static Settings _settings;
        private const string noTokenString = "YOUR_TELEGRAM_BOT_API_TOKEN";
        private const string settingsPathString = "settings.json";
        public string token;
        public int ownerId;
        public int chatId;

        public static Settings Instance()
        {
            if (_settings != null) return _settings;
            else return _settings = ReadSettings();
        }

        private static Settings ReadSettings()
        {
            try
            {
                var jsonText = File.ReadAllText(settingsPathString);
                var jsonObj = JsonConvert.DeserializeObject<Settings>(jsonText);
                if (jsonObj == null) throw new JsonException("Could not deserialize " + jsonText);
                else if (jsonObj.token == noTokenString) throw new FormatException("Please change default token in settings.json to your Telegram Bot API token");
                return jsonObj;
            }
            catch(Exception ex)
            {
                GarbageFunctionality.Log(ex);
                throw new Exception("Could not read settings", ex);
            }
        }

        private static void SaveSettings()
        {
            try
            {
                string jsonText = JsonConvert.SerializeObject(_settings);
                File.WriteAllText(settingsPathString, jsonText);
            }
            catch(Exception ex)
            {
                GarbageFunctionality.Log(ex);
                throw new Exception("Could not save settings", ex);
            }
        }

    }
}
