using System;
using System.IO;
using TinyJSON;

namespace ZergMod.Scripts.Data
{
    public static class DataUtil
    {
        public static T LoadFromFile<T>(string path) where T : AData
        {
            string fullPath = Path.Combine(Plugin.Directory, path);
            
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
            
            // Deserialize JSON
            JSON.MakeInto(JSON.Load(str), out T loadedData);
            if (loadedData == null)
            {
                Plugin.Log.LogWarning("Failed to load " + str);
            }
            else
            {
                loadedData.OnPostLoad();
            }

            return loadedData;
        }
    }
}