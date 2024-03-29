﻿using System;
using DiskCardGame;
using StarCraftCore.Scripts.SpecialAbilities;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class BrendaSpecialAbility : ACustomSpecialAbilityBehaviour<BrendaSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
        }
    }
}