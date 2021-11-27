using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZergMod
{
    public class CustomSaveManager
    {
        public static CustomSaveData SaveFile
        {
            get
            {
                if (m_customSaveData == null)
                {
                    LoadFromFile();
                    if (m_customSaveData == null)
                    {
                        m_customSaveData = new CustomSaveData();
                    }
                }

                return m_customSaveData;
            }
        }

        private static CustomSaveData m_customSaveData = null;
        private static string SaveFilePath
        {
            get
            {
                string fileName = Plugin.PluginName + "_SaveFile.gwsave";
                while (fileName.Contains(" "))
                {
                    fileName = fileName.Replace(" ", "");
                }
                return SaveManager.SaveFolderPath + fileName;
            }
        }

        public static void SaveToFile()
        {
            File.WriteAllText(CustomSaveManager.SaveFilePath, SaveManager.ToJSON(CustomSaveManager.SaveFile));
        }
        
        public static void LoadFromFile()
        {
            if (File.Exists(CustomSaveManager.SaveFilePath))
            {
                string json = File.ReadAllText(CustomSaveManager.SaveFilePath);
                try
                {
                    m_customSaveData = SaveManager.FromJSON<CustomSaveData>(json);
                    return;
                }
                catch
                {
                    m_customSaveData = null;
                    return;
                }
            }
        }
    }
    
    public class CustomSaveData
    {
        public int DehakaKills = 0;
    }
}