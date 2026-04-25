using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YINGYANG.Content.Buffs;
using YINGYANG.Content.npc;

namespace YINGYANG.Content.Projectiles
{
    public class DarkEyesLaserWallHitEffect : GlobalProjectile
    {
        public override bool InstancePerEntity => false;
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<DARK_EYES>()))
            {
                return;
            }
            bool isLaserWall = projectile.type == ModContent.ProjectileType<Dead_ray>() &&
                               (projectile.ai[1] == 2f || projectile.ai[1] == 3f);
            bool isSideDemonSickle = projectile.type == ProjectileID.DemonSickle && projectile.hostile;
            if (isLaserWall || isSideDemonSickle)
            {
                // 5s debuff when player is hit by laserWall package projectiles.
                target.AddBuff(ModContent.BuffType<Evil_aura>(), 60 * 5);
            }
        }
    }
}
