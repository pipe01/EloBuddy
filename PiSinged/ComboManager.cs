using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiSinged
{
    class ComboManager
    {
        private static Spell.Active Q;
        private static Spell.Skillshot W;
        private static Spell.Targeted E;
        private static Spell.Active R;

        public static void Init()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W,
                (uint)Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.CastRange,
                SkillShotType.Circular);
            E = new Spell.Targeted(SpellSlot.E,
                (uint)Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Active(SpellSlot.R);
        }

        public static void Combo(string combo, Obj_AI_Base target = null)
        {
            switch (combo)
            {
                case "ew":
                    if (Player.Instance.Position.IsInRange(target, E.Range))
                    {
                        E.Cast(target);
                        while (target.Position.Z > Player.Instance.Position.Z + 100) ;
                        W.Cast(target);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
