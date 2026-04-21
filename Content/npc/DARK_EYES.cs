using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using YINGYANG.Content.Projectiles;

namespace YINGYANG.Content.npc
{
    [AutoloadBossHead]
    public class DARK_EYES : ModNPC
    {
        private int ProjectileCooldown = 0;
        private int BossEscapeTimer;//逃脱计时器
        private int BossEscapeDelay = 180;//倒数计时器
        private int BossEscapeDistance = 3200;//逃脱距离
        private bool IsDespawning;//是否正在离场
        private int DespawnFlyTimer;//离场飞行计时
        private int DespawnFlyDuration = 120;//离场飞行时长（帧）
        private bool DashCalLock = false;//预判玩家动向锁
        private Microsoft.Xna.Framework.Vector2 DashVector;//冲刺向量
        private int LaserProjectileIndex = -1;//镭射ID
        private float LaserRotateAmount = 0f;//旋转的量
        private bool laserStateInitialized = false;//已发射镭射
        private const float LaserRotateStep = MathHelper.TwoPi / 180f;
        private int LaserDamage = 80;
        public override void SetStaticDefaults()//BOSS预设值
        {
            // Ensure Terraria treats this ModNPC as a real boss for UI/progression behavior.
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            Main.npcFrameCount[Type] = 1;
        }
        public override void SetDefaults()//预设值
        {
            NPC.boss = true;
            NPC.lifeMax = 30000;//经典模式
            NPC.damage = 100;//伤害
            NPC.aiStyle = -1;//不使用AI
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 200;

        }
        private enum BossState
        {
            Chase = 0,
            Dash = 1,
            ShootLaser = 2
        }
        private BossState CurrentBossState = BossState.Chase;
        private int BossStateTimer = 0;
        private int BossCoolDown = 0;
        public override void AI()
        {
            NPC.TargetClosest(faceTarget: false);//选最近玩家 同时不让NPC转向目标方向
            if(NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)//如果没有寻找到存活的玩家
            {
                Despawn();
                return;
            }
            Player player = Main.player[NPC.target];
            float distance = Microsoft.Xna.Framework.Vector2.Distance(NPC.Center, player.Center);
            if(distance > BossEscapeDistance)
            {
                BossEscapeTimer++;
                if(BossEscapeTimer >= BossEscapeDelay)
                {
                    Despawn();
                    return;
                }
            }
            else
            {
                BossEscapeTimer = 0;
            }
            //--------------------------------------BOSS SHOW(AI)--------------------------------------
            BossStateTimer++;
            if (CurrentBossState == BossState.Chase)
            {
                ProjectileCooldown++;
            }
            else
            {
                ProjectileCooldown = 0;
            }
            switch (CurrentBossState)
            {
                case BossState.Chase:
                    DoChase(player);
                    break;
                case BossState.Dash:
                    DoDash(player);
                    break;
                case BossState.ShootLaser:
                    Doshootlaser(player);
                    break;
            }
        }
        private void RandomBossState()
        {
            BossStateTimer = 0;
            BossCoolDown = 30;
            BossState next;
            do
            {
                next = (BossState)Main.rand.Next(0, 3);
            }
            while (next == CurrentBossState);

            CurrentBossState = next;
        }
        private void DoChase(Player target)
        {
            Microsoft.Xna.Framework.Vector2 toTarget = target.Center - NPC.Center;
            float chaseSpeed = 8f;//速度
            float SpeedUP = 0.2f;//加速度
            if (toTarget != Microsoft.Xna.Framework.Vector2.Zero)
            {
                toTarget.Normalize();
            }
            Microsoft.Xna.Framework.Vector2 ChaseVelocity = toTarget * chaseSpeed;//向量
            NPC.velocity = Microsoft.Xna.Framework.Vector2.Lerp(NPC.velocity, ChaseVelocity, SpeedUP);//NPC的速度
            if(ProjectileCooldown > 30)
            {
                ProjectileCooldown = 0;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, toTarget * 8f, ProjectileID.EyeBeam, 80, 2f);
            }
            // Keep spinning during chase.
            NPC.rotation += 0.4f;
            if (BossStateTimer >= 360)
            {
                NPC.rotation = 0;
                RandomBossState();
            }
        }
        private void Doshootlaser(Player target)
        {
            if(!laserStateInitialized)
            {
                laserStateInitialized = true;
                LaserRotateAmount = 0f;
                LaserProjectileIndex = -1;
                NPC.rotation = -MathHelper.PiOver2;
                NPC.velocity *= 0.2f;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Microsoft.Xna.Framework.Vector2 LasserDir = NPC.rotation.ToRotationVector2();
                    LaserProjectileIndex = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, LasserDir, ModContent.ProjectileType<Dead_ray>(), LaserDamage, 2f, Main.myPlayer, NPC.whoAmI, 1f);
                }
            }
            NPC.velocity *= 0;
            NPC.rotation += LaserRotateStep;
            bool LaserActive = LaserProjectileIndex >= 0 && LaserProjectileIndex < Main.maxProjectiles && Main.projectile[LaserProjectileIndex].active && Main.projectile[LaserProjectileIndex].type == ModContent.ProjectileType<Dead_ray>();
            if(!LaserActive)
            {
                RandomBossState();
                laserStateInitialized = false;
            }
        }
        private void CalculateDash(Player target)
        {
            if(!DashCalLock)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                float PredictTime = 30f;
                Microsoft.Xna.Framework.Vector2 PredictPos = target.Center + target.velocity * PredictTime;
                Microsoft.Xna.Framework.Vector2 BossDashDir = PredictPos - NPC.Center;
                if(BossDashDir == Microsoft.Xna.Framework.Vector2.Zero)
                {
                    BossDashDir = Microsoft.Xna.Framework.Vector2.UnitY;
                }
                BossDashDir.Normalize();
                float dashspeed = 22f;
                DashVector = BossDashDir * dashspeed;
                DashCalLock = true;
            }
        }
        private void DoDash(Player target)
        {
            if(!DashCalLock)
            {
                CalculateDash(target);
            }
            NPC.velocity = DashVector;
            if(BossStateTimer >= 30)
            {
                DashCalLock = false;
                BossStateTimer = 0;
                RandomBossState();
            }
        }
        public void Despawn()
        {
            NPC.velocity.Y = -10f;
            if(NPC.timeLeft > 10)
            {
                NPC.timeLeft = 10;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) //难度和玩家数量
        {
            if (Main.expertMode)//专家模式
            {
                NPC.lifeMax = 42000;
                NPC.damage = 57;
                LaserDamage = 90;
            }
            if (Main.masterMode)//大师模式
            {
                NPC.lifeMax = 54000;
                NPC.damage = 64;
                LaserDamage = 100;
            }

        }
    }
}

    
