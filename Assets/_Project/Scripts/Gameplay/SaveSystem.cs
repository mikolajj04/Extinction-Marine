using System;
using System.IO;
using UnityEngine;


namespace ExtinctionMarine.Gameplay.Save
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
    public static class SaveSystem
    {
        private static string SavePath => Application.persistentDataPath + "/marine_armory.json";
        public static void Save(ArmorySaveData data)
        {
            try { 
            string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"[SaveSystem] Data saved on the disk: {SavePath}");
            }
            catch(Exception ex)
            {
                Debug.Log($"[SaveSystem] I/O write error: {ex.Message}");
            }
        }

        public static ArmorySaveData Load()
        {
            try {
                if (File.Exists(SavePath)) { 
 

                    string json = File.ReadAllText(SavePath);
                    return JsonUtility.FromJson<ArmorySaveData>(json);
                }
            }
            catch(Exception ex)
            {
                Debug.Log($"[SaveSystem] The log file is corrupted or no permissions: {ex}");
            }
            return new ArmorySaveData();
        }

    }
}
