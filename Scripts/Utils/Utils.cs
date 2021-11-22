﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZergMod
{
    public static class Utils
    {
        private static List<Texture> s_defaultDecals = null;
        
        public static Texture2D GetTextureFromPath(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, path));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            return tex;
        }
        public static List<Texture> GetDecals()
        {
            if (s_defaultDecals == null)
            {
                Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);
                s_defaultDecals = new List<Texture> { decal };
            }
            
            return s_defaultDecals;
        }
    }
}