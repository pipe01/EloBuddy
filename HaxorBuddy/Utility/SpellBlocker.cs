using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    class SpellBlocker : Mode
    {
        private readonly Dictionary<string, SpellSlot> Champions = new Dictionary<string, SpellSlot>();

        private readonly SpellSlot[] SpellsSlots =
            { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R};
        Menu spellShield;

        private Spell.Active SpellShield;

        public static bool Debugging = false;

        public override void CreateMenu()
        {
            spellShield = HaxorMenu.haxorMenu.AddSubMenu("Spell Shield", "spellShield");
            spellShield.Add("debug", new CheckBox("Show debug info", false))
                .OnValueChange += (sender, args) => { Debugging = args.NewValue; };
            spellShield.AddGroupLabel("Enemy Champions");
            foreach (var item in EntityManager.Heroes.Enemies)
            {
                spellShield.AddLabel(item.ChampionName);
                foreach (var spell in item.Spellbook.Spells)
                {
                    if (!SpellsSlots.Contains(spell.Slot)) continue;
                    spellShield.Add(item.Name + spell.Name, new Slider(spell.SData.DisplayNameTranslated + " (" + spell.Slot.ToString() + ")", 50, 0, 100));
                }
                spellShield.AddSeparator();
            }
        }

        public override bool Init()
        {
            Champions.Clear();
            Champions.Add("sivir", SpellSlot.E);
            Champions.Add("nocturne", SpellSlot.W);

            Game.OnUpdate += Game_OnUpdate;
            Player.OnSpellCast += Player_OnSpellCast;
            
            string champname = Player.Instance.ChampionName.ToLower();
            if (!Champions.ContainsKey(champname))
            {
                Chat.Print("You didn't pick any champion with spell shield, Auto Spell Shielder won't work.");
                return false;
            }
            else
            {
                SpellShield = new Spell.Active(Champions[champname]);
            }
            return true;
        }

        public override void Stop()
        {
            Game.OnUpdate -= Game_OnUpdate;
            Player.OnSpellCast -= Player_OnSpellCast;
        }

        public override string GetID()
        {
            return "Spell Blocker";
        }

        public override bool DefaultEnabled()
        {
            return false;
        }

        private void Player_OnSpellCast(Obj_AI_Base ser, GameObjectProcessSpellCastEventArgs a)
        {
            Obj_AI_Base sender = ser;
            SpellData sdata = a.SData;

            if (!(sender is AIHeroClient)) return;

            AIHeroClient hero = (AIHeroClient)sender;

            if (sender.IsMe || sender.Team == Player.Instance.Team) return;

            if (IsAutoAttack(a.Slot)) return;

            if (Debugging) Chat.Print(hero.ChampionName + " " + a.Slot.ToString());
            
                  

        }

        private bool IsAutoAttack(SpellSlot slot)
        {
            string s = slot.ToString();
            int i;
            bool ret = int.TryParse(s, out i);
            return ret;
        }

        private void UseSpellShield()
        {
            SpellShield.Cast();
        }

        private void Game_OnUpdate(EventArgs args)
        {
            
        }
    }
}
