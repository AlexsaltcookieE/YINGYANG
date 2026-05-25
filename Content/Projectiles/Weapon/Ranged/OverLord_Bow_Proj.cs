using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace YINGYANG.Content.Projectiles.Weapon.Ranged
{
    public class OverLord_Bow_Proj : ModProjectile
    {
        private const int TotalFrames = 8;   // 总帧数
        private const int FrameSpeed = 5;    // 每几帧切一次

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = TotalFrames;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;          // 对玩家友好
            Projectile.hostile = false;          // 不对玩家造成伤害
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 3f;
        }

        public override void AI()
        {
            // 帧动画逻辑
            if (++Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= TotalFrames)
                    Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / TotalFrames;
            Rectangle sourceRect = new Rectangle(
                0,
                Projectile.frame * frameHeight,
                texture.Width,
                frameHeight
            );
            Vector2 origin = sourceRect.Size() / 2f;
            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            return false;
        }
    }
}