using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Rendering;

namespace HaxorBuddy
{
    class LineTracker : Mode
    {
        private int Distance
        {
            get
            {
                return HaxorMenu.haxorMenu.Get<Slider>("ltDistance").CurrentValue;
            }
        }

        public override bool Init()
        {
            Drawing.OnDraw += Drawing_OnDraw;
            return true;
        }

        private void clickedHappened()
        {
            Chat.Print(Game.CursorPos2D.X + "-" + Game.CursorPos2D.Y);
        }

        public override void Stop()
        {
            Drawing.OnDraw -= Drawing_OnDraw;
        }

        public override string GetID()
        {
            return "Line Tracker";
        }

        public override void CreateMenu()
        {
            HaxorMenu.haxorMenu.AddGroupLabel("Line tracker");
            HaxorMenu.haxorMenu.Add("ltDistance", new Slider("Minimum distance", 3000, 1, 10000));
        }

        public override bool DefaultEnabled()
        {
            return true;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            Color lineColor;

            foreach (AIHeroClient item in VisibleHeroes())
            {
                lineColor = item.Team == Player.Instance.Team ? Color.DarkGreen : Color.DarkRed;
                Line.DrawLine(lineColor, Player.Instance.Position, item.Position);
            }

            if (Program.EnabledModes.ContainsKey("Last Known Position"))
            {
                foreach (var item in ((Awareness.LastKnownPosition)
                    Program.Modes["Last Known Position"]).Positions.Values.Where
                    (o => o.WorldPosition.IsInRange(Player.Instance, Distance)))
                {
                    Line.DrawLine(Color.Gray, Player.Instance.Position, item.WorldPosition);
                }
            }
        }

        private AIHeroClient[] VisibleHeroes()
        {
            return EntityManager.Heroes.AllHeroes.Where
                    (o => o.IsInRange(Player.Instance, Distance) &&
                    !o.IsMe && !o.IsDead && o.IsVisible && o.IsHPBarRendered).ToArray();
        }
    }
}
