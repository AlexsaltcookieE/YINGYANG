using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using YINGYANG.Content.Tiles.Ore;

namespace YINGYANG.Content.WorldGenarate
{
    public class YINYANGWorldGenarateSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)//世界生成
        {
            //插入生成矿石的命令
            int OresIndex = tasks.FindIndex(pass => pass.Name.Equals("Shinies", StringComparison.OrdinalIgnoreCase));
            if(OresIndex != -1)
            {
                tasks.Insert(OresIndex + 1, new PassLegacy("YINGYANG Ores",GenerateCyan_Fe));
            }
        }
        private void GenerateCyan_Fe(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "生成一些带有仙气的矿石";
            int OreTileType = ModContent.TileType<Cyan_Fe>();
            int OreVein = (int)(Main.maxTilesX * Main.maxTilesY * 0.00006f);//矿脉密度
            //矿脉生成层级
            int MinLayerY = (int)Main.rockLayer;
            int MaxLayerY = Main.maxTilesY - 200;
            //Gen
            for (int i = 0 ; i < OreVein; i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(MinLayerY,MaxLayerY);
                double VeinThickness = WorldGen.genRand.Next(4, 8);
                int VeinLength = WorldGen.genRand.Next(3, 7);
                WorldGen.TileRunner(x, y, VeinThickness, VeinLength, OreTileType);
            }

        }
    }
}