using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy.Graphical
{
    class SpellRanges : Mode
    {
        public override void CreateMenu()
        {
            
        }

        public override bool DefaultEnabled()
        {
            return true;
        }

        public override string GetID()
        {
            return "Show Ranges";
        }

        public override bool Init()
        {
            Drawing.OnDraw += Drawing_OnEndScene;
            return true;
        }

        private void Drawing_OnEndScene(EventArgs args)
        {
            Drawing.DrawCircle(Player.Instance.Position, Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.CastRange, System.Drawing.Color.Red);
            Drawing.DrawCircle(Player.Instance.Position, Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.CastRange, System.Drawing.Color.Red);
            Drawing.DrawCircle(Player.Instance.Position, Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.CastRange, System.Drawing.Color.Red);
            Drawing.DrawCircle(Player.Instance.Position, Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.CastRange, System.Drawing.Color.Red);
        }

        public override void Stop()
        {
            Drawing.OnDraw -= Drawing_OnEndScene;
        }
    }
}
