using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using YINGYANG.Content.Projectiles;

namespace YINGYANG.Content.npc.Hungry_Ghost_Festival
{
    [AutoloadBossHead]
    public class Captain_Army_Ghost : ModNPC
    {
        private bool Cal = false;
        private int CalDir;
        private int ProjectCoolTimer;
        private bool Thorn = false;
        private bool BossLeftSide;
        private int BossEscapeTimer;//逃脱计时器
        private int BossEscapeDelay = 180;//倒数计时器
        private int BossEscapeDistance = 3200;//逃脱距离
        private const int FrameSpeed = 6;
        private int FrameCounter;
        private BossState CurrentBossState = BossState.Idle;
        private int BossStateTimer = 0;
        private int BossCoolDown = 0;
        public override void SetStaticDefaults()//BOSS预设值
        {
            // Ensure Terraria treats this ModNPC as a real boss for UI/progression behavior.
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            Main.npcFrameCount[Type] = 3;
        }
        private enum BossState
        {
            Idle = 0,
            Move = 1,
            Attack = 2
        }
        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.width = 20;
            NPC.height = 32;
            NPC.damage = 20;
            NPC.defense = 8;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.scale = 2f;
        }
        private int BossDir()
        {
            Player player = Main.player[NPC.target];
            if (NPC.Center.X >= player.Center.X)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public override void AI()
        {
            BossStateTimer++;
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X >= NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest(faceTarget: false);//选最近玩家 同时不让NPC转向目标方向
            if(NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)//如果没有寻找到存活的玩家
            {
                Despawn();
                return;
            }
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
            //----------------------------------Ai----------------------------------
            ProjectCoolTimer++;
            switch (CurrentBossState)
            {
                case BossState.Idle:
                    DoIdle(player);
                    break;
            }
        }
        public void Despawn()
        {
            NPC.velocity.Y = -10f;
            if (NPC.timeLeft > 10)
            {
                NPC.timeLeft = 10;
            }
        }
        private void RandomBossState(BossState? blockedState = null)
        {
            BossStateTimer = 0;
            BossCoolDown = 30;
            BossState next;
            do
            {
                next = (BossState)Main.rand.Next(0, 3); // 0..2 for Idle, Move, Attack
            }
            while (next == CurrentBossState || (blockedState.HasValue && next == blockedState.Value));
            CurrentBossState = next;
        }
        public override void OnKill()
        {
            //在BOSS死亡时执行的代码
            //例如：掉落物品、触发事件等
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            //在BOSS受到伤害时执行的代码
            //例如：生成特效、播放声音等
        }

        private void DoIdle(Player target)
        {
            //执行Idle状态的行为
            if (BossStateTimer < 120)
            {
                Microsoft.Xna.Framework.Vector2 ProjectileToTarget = target.Center - NPC.Center;
                Microsoft.Xna.Framework.Vector2 ThornVector;
                if (ProjectileToTarget != Microsoft.Xna.Framework.Vector2.Zero)
                {
                    ProjectileToTarget.Normalize();
                }
                if (ProjectCoolTimer >= 60 && !Thorn)
                {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,ProjectileToTarget, ModContent.ProjectileType<Cyan_Dragon>(), 10, 1f, Main.myPlayer, NPC.whoAmI, 2f);
                        Thorn = true;
                        if (Thorn)
                        {
                        }
                    }
                }
                else
                {
                    BossStateTimer = 0;
                    RandomBossState();
                }
                //例如：站立、播放动画等
            }
        public override void FindFrame(int frameHeight)
        {
            FrameCounter++;
            if (FrameCounter >= FrameSpeed)
            {
                FrameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
                {
                    NPC.frame.Y = 0;
                }

            }
        }
    }
}
