using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using YINGYANG.Content.Items.BossSummonItem;
using YINGYANG.Content.npc;

namespace YINGYANG.Content.ModAddons
{
    public class BossChecklistAddons : ModSystem
    {
        public static bool DarkEyesDowned;

        public override void PostSetupContent()
        {
           // Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            //if (bossChecklist == null)
            //{
            //    return;
            //}

           // try
            //{
          //      bossChecklist.Call(
           //         "AddBoss",
            //        ModContent.NPCType<DARK_EYES>(),
            //        5.1f,
            //        "魔尊之眼",
             //       "阴阳",
            //        (Func<bool>)(() => DarkEyesDowned),
             //       new List<int> { ModContent.ItemType<SummonDark_eye>() },
             //       new List<int> { ModContent.ItemType<SummonDark_eye>() }
             //   );
           // }
           // catch (Exception ex)
            //{
            //    Mod.Logger.Warn($"[YINGYANG] BossChecklist AddBoss failed: {ex.Message}");
           // }
        }
    }
}
