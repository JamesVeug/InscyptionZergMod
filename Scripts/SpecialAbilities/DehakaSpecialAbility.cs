using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using ZergMod;
namespace DiskCardGame
{
    public class DehakaSpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        private bool m_activated = false;

        public static void Initialize()
        {
            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID("DehakaSpecialAbility", "DehakaSpecialAbility");
            
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = "Essence Steal";
            iconInfo.rulebookDescription = "When a card with this sigil kills another stronger card it will steal its essence and grow in Power or Health.";
            iconInfo.iconType = SpecialStatIcon.CardsInHand;
            iconInfo.iconGraphic = Utils.GetTextureFromPath("Artwork/dehaka.png");
            iconInfo.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };
            
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(DehakaSpecialAbility), identifier, iconInfo);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            if (m_activated || !target.Dead)
            {
                return false;
            }

            if (target.Info.Health >= Card.Info.Health)
            {
                return true;
            }
            if (target.Info.Attack >= Card.Info.Attack)
            {
                return true;
            }

            return false;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            m_activated = true;
            CustomSaveManager.SaveFile.DehakaKills++;
            yield break;
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return m_activated;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            m_activated = false;
            yield break;
        }
    }
}