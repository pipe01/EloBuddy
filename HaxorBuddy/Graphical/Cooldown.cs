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
    class Cooldown : Mode
    {
        public override void Init()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;
            this.text = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold));
        }

        public override void Stop()
        {
            Drawing.OnEndScene -= Drawing_OnEndScene;
        }

        public string GetID()
        {
            return "cooldown";
        }

        private Text text;

        private readonly SpellSlot[] SpellsSlots =
            { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R};

        private void Drawing_OnEndScene(EventArgs args)
        {
            if (!Loading.IsLoadingComplete) return;
            if (HaxorMenu.haxorMenu["hmEnemyCD"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var item in EntityManager.Heroes.Enemies.Where(o => o.IsHPBarRendered))
                {
                    foreach (var spell in SpellsSlots)
                    {
                        this.DrawSpell(item, spell);
                    }
                }
            }
            if (HaxorMenu.haxorMenu["hmAllyCD"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var item in EntityManager.Heroes.Allies.Where(o => o.IsHPBarRendered))
                {
                    foreach (var spell in SpellsSlots)
                    {
                        this.DrawSpell(item, spell);
                    }
                }
            }
        }

        private void DrawSpell(AIHeroClient hero, SpellSlot slot)
        {
            var spell = hero.Spellbook.GetSpell(slot);
            var color = Color.Green;
            var cooldown = spell.CooldownExpires - Game.Time;
            var location = GetSpellLocation(hero, slot);

            var str = slot.ToString();

            if (!spell.IsLearned)
            {
                str = "X";
                color = Color.Red;
            }
            else if (hero.Mana < spell.SData.Mana)
            {
                str = "M";
                color = Color.LightBlue;
            }
            else if (cooldown > 0)
            {
                str = ((int)Math.Ceiling(cooldown)).ToString();
                color = Color.Orange;
            }

            this.text.Draw(string.Format("{0,3}", str), color, (int)location.X, (int)location.Y);
        }
        private Vector2 GetSpellLocation(AIHeroClient hero, SpellSlot slot)
        {
            var gap = 27;
            var x = hero.HPBarPosition.X + (hero.IsMe ? 34 : 0) + AdditionalXOffset(hero);
            var y = hero.HPBarPosition.Y + 23;

            switch (slot)
            {
                case SpellSlot.Q:
                    return new Vector2(x, y);
                case SpellSlot.W:
                    return new Vector2(x + gap, y);
                case SpellSlot.E:
                    return new Vector2(x + 2 * gap, y);
                case SpellSlot.R:
                    return new Vector2(x + 3 * gap, y);
                //case SpellSlot.Summoner1:
                //    return new Vector2(x, y - 23);
                //case SpellSlot.Summoner2:
                //    return new Vector2(x, y - 8);
            }

            return Vector2.Zero;
        }

        private float AdditionalXOffset(AIHeroClient hero)
        {
            var champName = hero.ChampionName.ToLower();

            switch (champName)
            {
                case "darius":
                    return -4;
            }

            return 0;
        }
    }
}
