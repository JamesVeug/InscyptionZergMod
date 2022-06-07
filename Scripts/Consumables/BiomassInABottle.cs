using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Consumables
{
    public class BiomassInABottleItem : ConsumableItem
    {
        // Token: 0x06000FB2 RID: 4018 RVA: 0x0003819A File Offset: 0x0003639A
        public override IEnumerator ActivateSequence()
        {
            base.PlayExitAnimation();
            yield return base.StartCoroutine(Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(this.cardInfo.name), 0.25f));
            yield return new WaitForSeconds(0.25f);
            yield break;
        }

        // Token: 0x04000B6D RID: 2925
        [SerializeField]
        private CardInfo cardInfo;
    }
}