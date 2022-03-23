using System;
using System.Collections;
using System.IO;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
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
	            BeforeSprite = beforeTexture.ConvertTexture(TextureHelper.SpriteType.CardPortrait);
	            
	            // Before Emit Texture
	            Texture2D beforeEmitTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_push_emit.png");
	            beforeEmitTexture.name = BeforeSprite.name + "_emission";
	            beforeEmitTexture.filterMode = FilterMode.Point;
                
	            Sprite beforeEmissiveSprite = beforeEmitTexture.ConvertTexture(TextureHelper.SpriteType.CardPortrait);
	            beforeEmissiveSprite.name = beforeTexture.name + "_emission";
	            beforeEmissiveSprite.RegisterEmissionForSprite(beforeEmitTexture, TextureHelper.SpriteType.CardPortrait);
	            
	            // After
	            Texture2D afterTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_after.png");
	            afterTexture.name = "portrait_brend_after";
	            AfterSprite = afterTexture.ConvertTexture(TextureHelper.SpriteType.CardPortrait);
	            
	            // After Emit Texture
	            Texture2D afterEmitTexture = Utils.GetTextureFromPath("Artwork/Cards/brenda_after_emit.png");
	            afterEmitTexture.name = afterTexture.name + "_emission";
	            afterEmitTexture.filterMode = FilterMode.Point;
                
	            Sprite afterEmissiveSprite = afterEmitTexture.ConvertTexture(TextureHelper.SpriteType.CardPortrait);
	            afterEmissiveSprite.name = afterTexture.name + "_emission";
	            afterEmissiveSprite.RegisterEmissionForSprite(afterEmitTexture, TextureHelper.SpriteType.CardPortrait);
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
	        CardInfo creepTumorInfo = ScriptableObjectLoader<CardInfo>.AllData.Find((CardInfo info) => info.name == "Zerg_JSON_CreepTumor");

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