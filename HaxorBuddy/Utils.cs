using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    class Utils
    {
        public static bool IsHeroInScreen(AIHeroClient hero)
        {
            Vector2 heroPos = hero.Position.To2D();
            return Vector2.Distance(heroPos, Player.Instance.Position.To2D()) < 3000;
        }

        public static Vector2 GetSpellLocation(AIHeroClient hero, SpellSlot slot)
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
            }

            return Vector2.Zero;
        }

        public static float AdditionalXOffset(AIHeroClient hero)
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
