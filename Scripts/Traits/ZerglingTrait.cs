using DiskCardGame;
using InscryptionAPI.Guid;

namespace ZergMod.Scripts.Traits
{
	public class ZerglingTrait
	{
		public static Trait ID;
		
		public static void Initialize()
		{
			ID = GuidManager.GetEnumValue<Trait>(Plugin.PluginGuid, "ZerglingTrait");
		}
	}
}