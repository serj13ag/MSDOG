using Newtonsoft.Json;
using UnityEngine;

namespace Services
{
    public class SaveLoadService
    {
        public T Load<T>(string key) where T : new()
        {
            var json = PlayerPrefs.GetString(key);
            return string.IsNullOrEmpty(json)
                ? new T()
                : JsonConvert.DeserializeObject<T>(json);
        }

        public void Save<T>(T saveData, string key)
        {
            var json = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}