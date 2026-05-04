using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YINGYANG.Content.Tiles;

namespace YINGYANG.Content.Items.Placeables
{
	public class Bloody_Chest_item : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Bloody_Chest>());
			// Item.placeStyle = 1; // Use this to place the chest in its locked style
			Item.width = 26;
			Item.height = 22;
			Item.scale = 1.5f;
			Item.value = 500;
			Item.useStyle = -1; // Prevents the player from using the item like a normal tile
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
}