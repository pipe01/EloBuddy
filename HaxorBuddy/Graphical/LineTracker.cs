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

namespace HaxorBuddy
{
    class LineTracker : Mode
    {
        public override void Init()
        {
            Drawing.OnDraw += Drawing_OnDraw;
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

        private void Drawing_OnDraw(EventArgs args)
        {
            if (Loading.IsLoadingComplete)
            {
                Color lineColor;

                foreach (AIHeroClient item in VisibleHeroes())
                {
                    lineColor = item.Team == Player.Instance.Team ? Color.DarkGreen : Color.DarkRed;
                    EloBuddy.SDK.Rendering.Line.DrawLine(lineColor, Player.Instance.Position, item.Position);
                }
            }
        }

        private AIHeroClient[] VisibleHeroes()
        {
            return EntityManager.Heroes.AllHeroes.Where
                    (o => o.IsInRange(Player.Instance,
                    HaxorMenu.haxorMenu.Get<Slider>("ltDistance").CurrentValue) &&
                    !o.IsMe && !o.IsDead && o.IsVisible && o.IsHPBarRendered).ToArray();
        }
    }
}
