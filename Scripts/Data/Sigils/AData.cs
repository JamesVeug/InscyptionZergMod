using System;
using System.Collections.Generic;
using DiskCardGame;

namespace ZergMod.Scripts.Data
{
    [Serializable]
    public class AData
    {
        public string name = "";
        public string ruleBookName = "";
        public string ruleDescription = "";
        public string iconPath = "Artwork/zerg.png";
        public List<AbilityMetaCategory> metaCategories = new List<AbilityMetaCategory>{ AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
    }
}