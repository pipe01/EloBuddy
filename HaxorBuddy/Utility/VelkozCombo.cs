using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy.Utility
{
    class VelkozCombo : Mode
    {
        private Menu vcMenu;

        private Spell.Skillshot Q;
        private SpellData QData;
        private Spell.Skillshot W;
        private SpellData WData;
        private Spell.Skillshot E;
        private SpellData EData;
        private Spell.Skillshot R;
        private SpellData RData;

        public override void CreateMenu()
        {
            vcMenu = HaxorMenu.haxorMenu.AddSubMenu("Vel'Koz Addon");

            vcMenu.AddGroupLabel("Keybinds");
            vcMenu.Add("kbUseCombo", new KeyBind("Use combo", false, KeyBind.BindTypes.HoldActive, 'T'));
        }

        public override bool DefaultEnabled()
        {
            return false;
        }

        public static string GetChampionName()
        {
            return "Velkoz";
        }

        public override string GetID()
        {
            return "Vel'Koz Combo";
        }

        public override bool Init()
        {
            if (Player.Instance.ChampionName != GetChampionName())
                return false;

            this.QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData;
            this.Q = new Spell.Skillshot(SpellSlot.Q, (uint)QData.CastRange,
                EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250,
                (int)QData.MissileSpeed, (int)QData.LineWidth);

            this.WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            this.W = new Spell.Skillshot(SpellSlot.W, (uint)WData.CastRange,
                EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250,
                (int)WData.MissileSpeed, (int)WData.LineWidth);

            this.EData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            this.E = new Spell.Skillshot(SpellSlot.W, (uint)EData.CastRange,
                EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250,
                (int)EData.MissileSpeed, (int)EData.LineWidth);

            this.RData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            this.R = new Spell.Skillshot(SpellSlot.R, (uint)RData.CastRange,
                EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250,
                (int)RData.MissileSpeed, (int)RData.LineWidth);

            Game.OnTick += Game_OnTick;

            return true;
        }

        private void Game_OnTick(EventArgs args)
        {
            if (vcMenu["kbUseCombo"].Cast<KeyBind>().CurrentValue)
            {
                DoCombo();
            }
        }

        private void DoCombo()
        {
            if (!E.IsReady()) return;

            var cursorPos = Game.CursorPos;

            Obj_AI_Base target = EntityManager.Heroes.Enemies.FirstOrDefault(o => o.Position.IsInRange(cursorPos, 250));

            if (target == null) return;

            PredictionResult ret = E.GetPrediction(target);

            if (ret.HitChancePercent >= 70)
            {
                var maxdmg = CalculateTotalDamage(target);
                var useult = maxdmg > target.Health;
                Chat.Print("Maximum combo damage: " + maxdmg);
                Q.Cast(target);
                W.Cast(target);
                E.Cast(target);
                if (useult) R.Cast(R.GetPrediction(target).CastPosition);
            }
        }

        private float CalculateTotalDamage(Obj_AI_Base target)
        {
            float ret = 0.0f;

            if (Q.IsReady())
                ret += Player.Instance.CalculateDamageOnUnit
                    (target, DamageType.Magical, GetQDamage(GetSDataInst(SpellSlot.Q).Level));

            if (W.IsReady())
                ret += Player.Instance.CalculateDamageOnUnit
                    (target, DamageType.Magical, GetQDamage(GetSDataInst(SpellSlot.W).Level));

            if (E.IsReady())
                ret += Player.Instance.CalculateDamageOnUnit
                    (target, DamageType.Magical, GetQDamage(GetSDataInst(SpellSlot.E).Level));

            if (R.IsReady())
                ret += Player.Instance.CalculateDamageOnUnit
                    (target, DamageType.Magical, GetQDamage(GetSDataInst(SpellSlot.R).Level));

            return ret;
        }

        private SpellDataInst GetSDataInst(SpellSlot slot)
        {
            return Player.Instance.Spellbook.GetSpell(slot);
        }

        private float GetQDamage(int level)
        {
            float basedmg = new float[] { 80, 120, 160, 200, 240 }[level - 1];
            return (float)(basedmg + Player.Instance.TotalMagicalDamage * 0.6);
        }

        private float GetWMaxDamage(int level)
        {
            float basedmg = new float[] { 75, 125, 175, 225, 275 }[level - 1];
            return (float)(basedmg + Player.Instance.TotalMagicalDamage * 0.625);
        }

        private float GetEDamage(int level)
        {
            float basedmg = new float[] { 70, 100, 130, 160, 190 }[level - 1];
            return (float)(basedmg + Player.Instance.TotalMagicalDamage * 0.5);
        }

        private float GetRMaxDamage(int level)
        {
            float basedmg = new float[] { 500, 700, 900 }[level - 1];
            return (float)(basedmg + Player.Instance.TotalMagicalDamage * 0.6);
        }

        public override void Stop()
        {

        }
    }
}
