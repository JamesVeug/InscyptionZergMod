using System.IO;
using UnityEngine;

namespace ZergMod
{
    public class Utils
    {
        public static Texture2D GetTextureFromPath(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, path));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            return tex;
        }
    }
}