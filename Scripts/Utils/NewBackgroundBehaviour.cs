using System;
using System.Collections.Generic;
using DiskCardGame;

namespace ZergMod
{
    public class NewBackgroundBehaviour
    {
        public static Dictionary<CardAppearanceBehaviour.Appearance, NewBackgroundBehaviour> Behaviours = new Dictionary<CardAppearanceBehaviour.Appearance, NewBackgroundBehaviour>();
        public static List<NewBackgroundBehaviour> AllBehaviours = new List<NewBackgroundBehaviour>();

        public CardAppearanceBehaviour.Appearance Appearance;
        public Type Behaviour;

        public static NewBackgroundBehaviour AddNewBackground(Type type)
        {
            var backgroundBehaviour = new NewBackgroundBehaviour();
            backgroundBehaviour.Appearance = (CardAppearanceBehaviour.Appearance)((int)(CardAppearanceBehaviour.Appearance.SexyGoat) + AllBehaviours.Count);
            backgroundBehaviour.Behaviour = type;
            
            Behaviours[backgroundBehaviour.Appearance] = backgroundBehaviour;
            AllBehaviours.Add(backgroundBehaviour);

            return backgroundBehaviour;
        }
    }
}