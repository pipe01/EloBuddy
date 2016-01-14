using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace HaxorBuddy
{
    class Experience : Mode
    {
        public override bool Init()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
            this.text = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold));
            return true;
        }

        public override void Stop()
        {
            Drawing.OnEndScene -= Drawing_OnEndScene;
        }

        public override string GetID()
        {
            return "Experience";
        }

        public override void CreateMenu()
        {
            HaxorMenu.haxorMenu.AddGroupLabel("Experience");
            HaxorMenu.haxorMenu.Add("hmAllyExp", new CheckBox("Show ally experience", false));
            HaxorMenu.haxorMenu.Add("hmEnemyExp", new CheckBox("Show enemy experience", true));
        }

        public override bool DefaultEnabled()
        {
            return true;
        }

        private Text text;

        private void Drawing_OnEndScene(EventArgs args)
        {
            if (Loading.IsLoadingComplete)
            {
                if (HaxorMenu.haxorMenu.Get<CheckBox>("hmEnemyExp").CurrentValue)
                {
                    foreach (var item in EntityManager.Heroes.Enemies.Where(o => o.IsHPBarRendered))
                    {
                        int expPerc = (int)Math.Ceiling(item.Experience.XPPercentage);
                        
                        text.Draw(expPerc.ToString() + "%", Color.Gold, GetExpLocation(item));
                    }
                }

                if (HaxorMenu.haxorMenu.Get<CheckBox>("hmAllyExp").CurrentValue)
                {
                    foreach (var item in EntityManager.Heroes.Allies.Where(o => o.IsHPBarRendered))
                    {
                        int expPerc = (int)Math.Ceiling(item.Experience.XPPercentage);

                        text.Draw(expPerc.ToString() + "%", Color.Gold, GetExpLocation(item));
                    }
                }
            }
        }

        private Vector2 GetExpLocation(AIHeroClient hero)
        {
            float x, y;
            if (hero.IsMe)
            {
                x = hero.HPBarPosition.X + 3;
                y = hero.HPBarPosition.Y + 24;
            }
            else
            {
                x = hero.HPBarPosition.X + 108;
                y = hero.HPBarPosition.Y + 26;
            }
            return new Vector2(x, y);
        }
    }
}
