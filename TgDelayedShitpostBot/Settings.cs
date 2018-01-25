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
        public string token;

        public static Settings Instance()
        {
            if (_settings != null) return _settings;
            else return _settings = ReadSettings();
        }

        private static Settings ReadSettings()
        {
            try
            {
                var jsonText = File.ReadAllText("settings.json");
                var jsonObj = JsonConvert.DeserializeObject<Settings>(jsonText);
                if (jsonObj == null) throw new JsonException("Could not deserialize " + jsonText);
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
                string jsonText = JsonConvert.SerializeObject(_settings, );
                File.WriteAllText("settings.json", jsonText);
            }
            catch(Exception ex)
            {
                GarbageFunctionality.Log(ex);
                throw new Exception("Could not save settings", ex);
            }
        }

    }
}
