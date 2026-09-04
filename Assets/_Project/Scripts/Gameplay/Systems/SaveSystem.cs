using System;
using System.IO;
using UnityEngine;


namespace ExtinctionMarine.Gameplay.Systems
{
    [Serializable]
    public class ArmorySaveData
    {
        public bool IsDashUnlocked = false;
        public bool IsGrenadeUnlocked = false;
        public bool IsShieldUnlocked = false;

        public int CarnotaurusKills = 0;
        public float LongestSurvivedTime = 0f;
    }

    [Serializable]
    public class SettingsSaveData
    {
        public float MasterVolume = 1.0f;
        public float MusicVolume = 1.0f;
        public float SfxVolume = 1.0f;
        public bool IsFullscreen = true;
    }

    public static class SaveSystem
    {
        public static void Save<T>(T data, string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            try { 
            string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
                Debug.Log($"[SaveSystem] Data ({typeof(T).Name}) saved on the disk: {path}");
            }
            catch(Exception ex)
            {
                Debug.Log($"[SaveSystem] I/O write error (file: {fileName}) Error : {ex.Message}");
            }
        }

        public static T Load<T>(string fileName) where T : new()
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            try {
                if (File.Exists(path)) { 
 

                    string json = File.ReadAllText(path);
                    return JsonUtility.FromJson<T>(json);
                }
            }
            catch(Exception ex)
            {
                Debug.Log($"[SaveSystem] The log file ({fileName}) is corrupted or no permissions: {ex}");
            }
            Debug.LogWarning($"[SaveSystem] Creating new file for: {typeof(T).Name}.");
            return new T();
        }

    }
}
