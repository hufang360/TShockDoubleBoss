using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace DoubleBoss
{
    [ApiVersion(2, 1)]
    public class DoubleBoss : TerrariaPlugin
    {
        # region 插件信息
        public override string Name => "DoubleBoss";
        public override string Description => "好事成双";
        public override string Author => "hufang360";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        #endregion

        readonly List<int> BossIDs = new List<int>(){
            50, // 史莱姆王
            4, // 克苏鲁之眼
            13, // 世界吞噬怪（14,15）
            266, // 克苏鲁之脑
            222, // 蜂王
            35, // 骷髅王
            668, // 鹿角怪
            113, // 血肉墙
            134, // 毁灭者（135,136）
            125, // 双子 激光眼 魔焰眼
            126, // 双子 魔焰眼
            127, // 机械骷髅王
            262, // 世纪之花
            245, // 石巨人
            657, // 史莱姆皇后
            636, // 光之女皇
            370, // 猪龙鱼公爵

            //395, // 火星飞碟
            398, // 月亮领主（396月亮领主，397 月亮领主手，398 月亮领主心脏）
            
            //664, // 火把神
            //517, // 日耀柱
            //422, // 星旋柱
            //507, // 星云柱
            //493, // 星尘柱

            439 // 教徒
        };

        public DoubleBoss(Main game) : base(game)
        {

        }

        public override void Initialize()
        {
            ServerApi.Hooks.NpcSpawn.Register(this, OnNpcSpawn);
        }

        private void OnNpcSpawn(NpcSpawnEventArgs args)
        {
            int id = Main.npc[args.NpcId].netID;
            if (!BossIDs.Contains(id)) return;
            if (!NeedDouble(id, out Point p)) return;

            NPC npc = new NPC();
            npc.SetDefaults(id);
            TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, 1, p.X, p.Y);
        }

        private bool NeedDouble(int id, out Point p)
        {
            p = new Point();
            int count = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == id)
                {
                    if (count > 0) return false;
                    count++;
                    p.X = (int)Main.npc[i].position.X / 16;
                    p.Y = (int)Main.npc[i].position.Y / 16;
                }
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NpcSpawn.Deregister(this, OnNpcSpawn);
            }
            base.Dispose(disposing);
        }
    }
}