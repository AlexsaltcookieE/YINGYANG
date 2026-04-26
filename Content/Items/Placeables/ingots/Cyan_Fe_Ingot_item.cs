using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YINGYANG.Content.Items.Placeables.CraftStations;
using YINGYANG.Content.Items.Placeables.Ores;
using YINGYANG.Content.Tiles.CraftStation;

namespace YINGYANG.Content.Items.Placeables.ingots
{
    public class Cyan_Fe_Ingot_item : ModItem
    {
        public override string Texture => "YINGYANG/Content/Items/Placeables/ingots/Cyan_Fe_Ingot_Item";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Cyan_Fe_Item>(), 5)
                .AddTile(ModContent.TileType<AlchemyFurnance>())
                .Register();
                
        }
    }
}
