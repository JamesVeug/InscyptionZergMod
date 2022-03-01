using System;
using System.Collections;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;
using ZergMod.Scripts.SpecialAbilities;

namespace ZergMod.Scripts.Abilities
{
	public class StrafeCreepTumorAbility : ACustomStrafe<StrafeCreepTumorAbility, StrafeCreepTumorData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;

		private static Sprite BeforeSprite = null;
		private static Sprite AfterSprite = null;
		private bool AnimatedBrenda = false;
		
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);

            if (BeforeSprite == null)
            {
	            // Before
	            Texture2D beforeTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_push.png");
	            beforeTexture.name = "portrait_brend_push";
	            BeforeSprite = Sprite.Create(beforeTexture, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
	            
	            // Before Emit Texture
	            Texture2D beforeEmitTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_push_emit.png");
	            beforeEmitTexture.name = BeforeSprite.name + "_emission";
	            beforeEmitTexture.filterMode = FilterMode.Point;
                
	            Sprite beforeEmissiveSprite = Sprite.Create(beforeEmitTexture, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
	            beforeEmissiveSprite.name = beforeTexture.name + "_emission";
	            NewCard.emissions.Add(beforeTexture.name, beforeEmissiveSprite);
	            
	            // After
	            Texture2D afterTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_after.png");
	            afterTexture.name = "portrait_brend_after";
	            AfterSprite = Sprite.Create(afterTexture, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
	            
	            // After Emit Texture
	            Texture2D afterEmitTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_after_emit.png");
	            afterEmitTexture.name = afterTexture.name + "_emission";
	            afterEmitTexture.filterMode = FilterMode.Point;
                
	            Sprite afterEmissiveSprite = Sprite.Create(afterEmitTexture, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
	            afterEmissiveSprite.name = afterTexture.name + "_emission";
	            NewCard.emissions.Add(afterEmitTexture.name, afterEmissiveSprite);
            }
        }

        public override IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
	        // Change image
	        bool isBrenda = this.Card.Info.SpecialAbilities.Contains(BrendaSpecialAbility.specialAbility); 
	        if (isBrenda && !AnimatedBrenda)
	        {
				this.Card.SwitchToPortrait(BeforeSprite);
				this.Card.RenderCard();
				yield return new WaitForSeconds(0.25f);
	        }
	        
	        yield return base.DoStrafe(toLeft, toRight);
	        
	        // Change image
	        if (isBrenda && !AnimatedBrenda)
	        {
		        this.Card.SwitchToPortrait(AfterSprite);
		        this.Card.RenderCard();
		        AnimatedBrenda = true;
		        yield return new WaitForSeconds(0.25f);
	        }
        }

        public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
	        CardInfo creepTumorInfo = ScriptableObjectLoader<CardInfo>.AllData.Find((CardInfo info) => info.name == "CreepTumor");

	        PlayableCard creepTumor = CardSpawner.SpawnPlayableCardWithCopiedMods(creepTumorInfo, base.Card, Ability.TailOnHit);
	        creepTumor.transform.position = oldSlot.transform.position + Vector3.back * 2f + Vector3.up * 2f;
	        creepTumor.transform.rotation = Quaternion.Euler(110f, 90f, 90f);
	        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(creepTumor, oldSlot, 0.1f, null, true);
	        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
	        yield return new WaitForSeconds(0.2f);
	        creepTumor.Anim.StrongNegationEffect();
	        yield return base.StartCoroutine(base.LearnAbility(0.5f));
	        yield return new WaitForSeconds(0.2f);
        }
	}
}