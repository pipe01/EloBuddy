using EloBuddy;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy.Awareness
{
    class WardTracker : Mode
    {
        private readonly string[] WardSpells = new[]
            { "ItemGhostWard", "VisionWard", "TrinketTotemLvl1" };

        private readonly int[] TrinketDurations = new[]
            { 60,64,67,71,74,78,81,85,88,92,95,99,102,106,109,113,116,120 };

        private string GetWardName(string wardID)
        {
            var names = new[] { "Invisible Ward", "Pink Ward", "Trinket" };
            var i = 0;
            foreach (var item in WardSpells)
            {
                if (wardID == item) return names[i];
                i++;
            }
            return null;
        }
        private int GetWardDuration(string wardID, int level = -1)
        {
            switch (wardID)
            {
                case "ItemGhostWard":
                    return 150;
                case "VisionWard":
                    return -1;
                case "TrinketTotemLvl1":
                    return TrinketDurations[level - 1];
                default:
                    return 0;
            }
        }

        private int GetMaxWardHP(string wardID)
        {
            switch (wardID)
            {
                case "ItemGhostWard":
                case "TrinketTotemLvl1":
                    return 3;
                case "VisionWard":
                    return 5;
                default:
                    return 0;
            }
        }

        private List<Ward> Wards = new List<Ward>();

        public override void CreateMenu()
        {
            
        }

        public override bool DefaultEnabled()
        {
            return true;
        }

        public override string GetID()
        {
            return "Ward Tracker";
        }

        public override bool Init()
        {
            Player.OnSpellCast += Player_OnSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnTick += Game_OnTick;
            
            return true;
        }

        private void Game_OnTick(EventArgs args)
        {
            for (int i = 0; i < Wards.Count; i++)
            {
                var item = Wards[i];
                if (item.TimeExpires <= Game.Time || item.Health == 0)
                {
                    Wards.Remove(item);
                }
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            foreach (var item in Wards)
            {
                var maxR = 27;
                var tleft = item.TimeExpires - Game.Time;
                var porc = (tleft / item.Duration) * 100;
                var radi = (porc / 100) * maxR + 23;

                Circle.Draw(new ColorBGRA(255, 0, 0, 255), 23, 2, item.Position);
                Circle.Draw(new ColorBGRA(255, 0, 0, 255), 50, 1, item.Position);

                Circle.Draw(new ColorBGRA(0, 0, 255, 255), (float)radi, item.Position);
            }
        }

        private void Player_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (WardSpells.Contains(args.SData.Name) && sender.Team != Player.Instance.Team)
            //if (WardSpells.Contains(args.SData.Name))
            {
                var level = ((AIHeroClient)sender).Level;
                float expires = Game.Time + GetWardDuration(args.SData.Name, level);
                var ward = new Ward(
                        sender.NetworkId,
                        args.End,
                        args.SData.Name,
                        sender.Team,
                        expires,
                        GetMaxWardHP(args.SData.Name),
                        GetWardDuration(args.SData.Name, level));
                Wards.Add(ward);
                //Chat.Print(GetWardDuration(args.SData.Name, level));
            } else if (sender.Team == Player.Instance.Team)
            {
                Ward w = null;

                try
                {
                    w = GetWardAt(args.Target.Position);
                }
                catch (Exception) { }

                if (w != null)
                {
                    w.Health--;
                }
            }
        }

        public override void Stop()
        {
            Player.OnSpellCast -= Player_OnSpellCast;
            Drawing.OnDraw -= Drawing_OnDraw;
            Game.OnTick -= Game_OnTick;
        }

        private bool IsWard(GameObject obj)
        {
            return obj.Type == GameObjectType.obj_AI_Minion && ((Obj_AI_Minion)obj).MaxHealth == 3;
        }

        private bool Exists(int netID)
        {
            return ObjectManager.GetUnitByNetworkId((uint)netID) == null;
        }

        private Ward GetWardAt(Vector3 pos)
        {
            foreach (var item in Wards)
            {
                if (item.Position == pos) return item;
            }
            return null;
        }

        private class Ward
        {
            public int SenderNetworkID;
            public Vector3 Position;
            public string WardType;
            public GameObjectTeam SenderTeam;
            public double TimeExpires;
            public int Duration;
            public int NetworkID;
            public int Health;

            public Ward(int snid, Vector3 pos, string wt, GameObjectTeam st, float e, int hp, int duration)
            {
                this.SenderNetworkID = snid;
                this.TimeExpires = e;
                this.Position = pos;
                this.WardType = wt;
                this.SenderTeam = st;
                this.Health = hp;
                this.Duration = duration;
            }
        }
    }
}
