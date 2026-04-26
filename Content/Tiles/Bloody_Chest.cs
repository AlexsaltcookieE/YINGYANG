using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace YINGYANG.Content.Tiles
{
    public class Bloody_Chest : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSpelunker[Type] = true;//洞穴药水高亮
            Main.tileContainer[Type] = true;//可以放置容器
            Main.tileShine2[Type] = true;//发光
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;//多格家具必须开启，否则会显示成一格一格的
            Main.tileNoAttach[Type] = true;//可以与其他物块连接
            Main.tileOreFinderPriority[Type] = 500;
            TileID.Sets.BasicChest[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

        }
    }
}