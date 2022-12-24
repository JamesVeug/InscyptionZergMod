using System.IO;
using DiskCardGame;
using InscryptionAPI.Items;
using InscryptionAPI.Masks;

namespace ZergMod.Scripts.Masks
{
	public class AbatharMask : MaskBehaviour
	{
		public static LeshyAnimationController.Mask ID;
		public static void Initialize()
		{
			ResourceLookup resourceLookup = new ResourceLookup();
			resourceLookup.FromAssetBundle(Path.Combine(Plugin.Directory, "AssetBundles/abathar_mask"), "AbatharMask_Mark3");

			MaskManager.ModelType modelType = MaskManager.RegisterPrefab(Plugin.PluginGuid, "AbatharMask", resourceLookup);
			ID = MaskManager.Add(Plugin.Directory, "AbatharMask").SetModelType(modelType).ID;
		}
	}
}