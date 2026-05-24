using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; // 引入绘图命名空间
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace YINGYANG.Content.Items.Weapons
{
    public class Overlord_Bow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 1. 开启挥动动画逻辑（这是必须的）
            // 2. 注册动画：指定动画类型为 Swing（挥动），并设置帧数为 8
            // 注意：这里的 8 代表总帧数，你需要确保你的贴图是竖排的 8 帧动画
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 8));

            // 显示名称 & 提示
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 48; // 注意：这里的 height 应该是单帧的高度，而不是整张图的高度
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;
            Item.scale = 2f;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 12;
            Item.knockBack = 1f;
            Item.crit = 4;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);

            Item.noMelee = true;
            Item.channel = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        // 可选：如果你需要更精细地控制每一帧的绘制（通常不需要，除非有特殊偏移）
        // public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor) { return true; }
        // public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) { return true; }
    }
}