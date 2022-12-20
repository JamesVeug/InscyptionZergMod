

namespace ZergMod.Scripts.Patches
{
    /*[HarmonyPatch(typeof(Opponent), nameof(Opponent.SpawnOpponent), new System.Type[] { typeof(EncounterData) })]
    public class Opponent_SpawnOpponent
    {
        public static bool Prefix(Opponent __instance, EncounterData encounterData)
        {
            encounterData.opponentType = MyBossOpponent.ID;
            Plugin.Log.LogInfo("[MapGenerator_CreateNode][Postfix]");
            return true;
        }
    }*/

    /*[HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.CreateNode),new System.Type[] {typeof(int), typeof(int), typeof(List<NodeData>), typeof(List<NodeData>), typeof(int)})]
    public class MapGenerator_CreateNode
    {
        public static void Postfix(ref NodeData __result)
        {
            BossBattleNodeData nodeData = new BossBattleNodeData();
            nodeData.gridX = __result.gridX;
            nodeData.gridY = __result.gridY;
            nodeData.id = __result.id;
            __result = nodeData;
            nodeData.bossType = MyBossOpponent.ID;
            nodeData.specialBattleId = BossBattleSequencer.GetSequencerIdForBoss(nodeData.bossType);
            Plugin.Log.LogInfo("[MapGenerator_CreateNode][Postfix]");
        }
    }*/

    /*[HarmonyPatch(typeof(MapDataReader), nameof(MapDataReader.SpawnAndPlaceElement),new System.Type[] {typeof(MapElementData), typeof(Vector2), typeof(bool)})]
    public class MapDataReader_SpawnAndPlaceElement
    {
        public static bool Prefix(ref MapElementData data, Vector2 sampleRange, bool isScenery)
        {
            Plugin.Log.LogInfo("[MapDataReader_SpawnAndPlaceElement][Prefix]");
            
            BossBattleNodeData nodeData = new BossBattleNodeData();
            nodeData.gridX = (int)data.position.x;
            nodeData.gridY = (int)data.position.y;
            nodeData.id = data.id;
            data = nodeData;
            nodeData.bossType = Opponent.Type.ProspectorBoss;
            nodeData.specialBattleId = BossBattleSequencer.GetSequencerIdForBoss(Opponent.Type.ProspectorBoss);

            return true;
        }
    }

    [HarmonyPatch(typeof(MapDataReader), nameof(MapDataReader.SpawnAndPlaceElement),new System.Type[] {typeof(MapElementData), typeof(Vector2), typeof(bool)})]
    public class MapDataReader_SpawnAndPlaceElement
    {
        public static bool Prefix(ref MapElementData data, Vector2 sampleRange, bool isScenery)
        {
            Plugin.Log.LogInfo("[MapDataReader_SpawnAndPlaceElement][Prefix]");
            
            BossBattleNodeData nodeData = new BossBattleNodeData();
            nodeData.gridX = (int)data.position.x;
            nodeData.gridY = (int)data.position.y;
            nodeData.id = data.id;
            data = nodeData;
            nodeData.bossType = Opponent.Type.ProspectorBoss;
            nodeData.specialBattleId = BossBattleSequencer.GetSequencerIdForBoss(Opponent.Type.ProspectorBoss);

            return true;
        }
    }*/
}