using System;
using System.IO;
using TinyJSON;

namespace ZergMod.Scripts.Data
{
    public static class DataUtil
    {
        public static T LoadFromFile<T>(string path) where T : AData
        {
            string fullPath = Path.Combine(Plugin.Directory, path + ".json");
            //Plugin.Log.LogInfo($"Loading {fullPath}");
            
            if (!File.Exists(fullPath))
            {
                Plugin.Log.LogError("File at path '" + fullPath + "' does not exist! Creating one now.");
                
                T t = (T)Activator.CreateInstance(typeof(T));
                string json = JSON.Dump(t, EncodeOptions.PrettyPrint);
                File.WriteAllText(fullPath, json);
                return t;
            }

            // Read JSON
            string text = File.ReadAllText(fullPath);
            string str = text.Substring(text.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            //Plugin.Log.LogInfo($"JSON: {str}");
            
            // Deserialize JSON
            JSON.MakeInto(JSON.Load(str), out T loadedData);
            //Plugin.Log.LogInfo($"Rulebook Name: {loadedData.ruleBookName}");
            
            if (loadedData == null)
            {
                Plugin.Log.LogWarning("Failed to load " + str);
            }

            return loadedData;
        }
    }
}