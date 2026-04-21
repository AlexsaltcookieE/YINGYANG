using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using YINGYANG.Content.Items;

namespace YINGYANG.Content.items
{
    public class PlayerStartLoot : ModPlayer
    {
        private bool hasReceivedStarterBag;

        public override void OnEnterWorld()
        {
            if (!hasReceivedStarterBag)
            {
                Player.QuickSpawnItem(Player.GetSource_GiftOrReward(), ModContent.ItemType<MysteryBag>());
                hasReceivedStarterBag = true;
            }
        }


        public override void SaveData(TagCompound tag)
        {
            tag["HasStarted"] = hasReceivedStarterBag;
        }

        public override void LoadData(TagCompound tag)
        {
            hasReceivedStarterBag = tag.GetBool("HasStarted"); //获取是否已获得
        }
    }
}
