﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    class AutoActivator : Mode
    {
        private bool Active
        {
            get { return aaMenu["aaActive"].Cast<CheckBox>().CurrentValue; }
        }

        private int ActHealth
        {
            get { return aaMenu["aaHealth"].Cast<Slider>().CurrentValue; }
        }

        private SpellSlot[] SummsSlots =
            { SpellSlot.Summoner1, SpellSlot.Summoner2 };

        private SpellSlot[] SpellSlots =
            { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R};

        private SpellSlot[] ItemSlots = {SpellSlot.Item1, SpellSlot.Item2,
            SpellSlot.Item3, SpellSlot.Item4, SpellSlot.Item5, SpellSlot.Item6 };

        private Dictionary<string, int> CastedSpells = new Dictionary<string, int>();

        private Menu aaMenu, usummMenu, usMenu, itMenu;
        private bool HasHeal, HasBarrier;

        private List<int> PlayerInventory = new List<int>();

        public override void Init()
        {
            CreateMenu();
            Game.OnUpdate += Game_OnUpdate;

        }

        public override void Stop()
        {
            Game.OnUpdate -= Game_OnUpdate;
        }

        public void CreateMenu()
        {
            aaMenu = HaxorMenu.haxorMenu.AddSubMenu("Auto Activator", "aaMenu");
            aaMenu.Add("aaActive", new CheckBox("Active", true));
            aaMenu.Add("aaDebug", new CheckBox("Show debug info", false));
            aaMenu.Add("aaHealth", new Slider("Health percentage at which to trigger", 30));

            usummMenu = HaxorMenu.haxorMenu.AddSubMenu("Summoner spells");
            usummMenu.AddGroupLabel("Summoner spells to use when below trigger health");
            if (GetSpellSlot("summonerheal") != SpellSlot.Unknown)
            {
                usummMenu.Add("summonerHeal", new Slider("Use summoner spell Heal", 0));
                HasHeal = true;
            }
            if (GetSpellSlot("summonerbarrier") != SpellSlot.Unknown)
            {
                usummMenu.Add("summonerBarrier", new Slider("Use summoner spell Barrier", 0));
                HasBarrier = true;
            }

            usMenu = HaxorMenu.haxorMenu.AddSubMenu("Champion spells", "champSpells");
            usMenu.AddGroupLabel("Spells to cast when below trigger health");
            foreach (var item in Player.Instance.Spellbook.Spells.Where
                (o => SpellSlots.Contains(o.Slot)))
            {
                usMenu.Add("spell" + item.Slot.ToString(), new Slider("Use slot " + item.Slot.ToString(), 0));
            }

            itMenu = HaxorMenu.haxorMenu.AddSubMenu("Items", "items");
            itMenu.AddGroupLabel("Health percentage at which to trigger items");
            itMenu.AddLabel("If any item is targeted, it will be used on self");
            for (int i = 1; i < 8; i++)
            {
                itMenu.Add("islot" + i, new Slider("Use item on slot " + i, 20));
            }
        }

        private SpellSlot GetSpellSlot(string spellname, AIHeroClient target)
        {
            foreach (var item in target.Spellbook.Spells)
            {
                if (item.Name == spellname) return item.Slot;
            }
            return SpellSlot.Unknown;
        }
        private SpellSlot GetSpellSlot(string spellname)
        {
            return GetSpellSlot(spellname, Player.Instance);
        }

        private bool CanCast(SpellSlot slot)
        {
            bool cancast = false;

            if (Player.Instance.Spellbook.GetSpell(slot).IsOnCooldown)
                cancast = false;

            return cancast;
        }
        private bool CanCast(string slot)
        {
            return CanCast(GetSpellSlot(slot));
        }

        private int OldHP = 100;
        private int HP;
        private void Game_OnUpdate(EventArgs args)
        {
            OldHP = HP;
            HP = (int)Player.Instance.HealthPercent;

            //if (OldHP != HP)
            //    Chat.Print("OLD: " + OldHP + " NEW: " + HP, System.Drawing.Color.Red);

            if (HP < OldHP)
            {
                UseItems((int)Player.Instance.HealthPercent);
                UseSpells((int)Player.Instance.HealthPercent);
            }
            else if (HP > OldHP && CastedSpells.Count > 0)
            {
                CastedSpells.Clear();
            }
        }

        private void UseItems(int perc)
        {
            for (int i = 1; i < 8; i++)
            {
                if (itMenu["islot" + i].Cast<Slider>().CurrentValue >= perc && 
                    !CastedSpells.Keys.Contains("islot" + i))
                {
                    Player.Instance.InventoryItems[i - 1].Cast(Player.Instance);
                    CastedSpells.Add("islot" + i, perc);
                }
            }
        }

        private void UseSpells(int perc)
        {
            foreach (var item in SpellSlots)
            {
                if (usMenu["spell" + item.ToString()].Cast<Slider>().CurrentValue >= perc &&
                    !CastedSpells.Keys.Contains("spell" + item.ToString()) &&
                    CanCast(item))
                {
                    Chat.Print("Trigering " + item.ToString() + "..", System.Drawing.Color.LightGreen);
                    Player.Instance.Spellbook.CastSpell(item, Player.Instance);
                    CastedSpells.Add("spell" + item.ToString(), perc);
                }
            }

            if (HasHeal)
            {
                if (usummMenu["summonerHeal"].Cast<Slider>().CurrentValue >= perc &&
                    CanCast("summonerheal"))
                {
                    Player.Instance.Spellbook.CastSpell(GetSpellSlot("summonerheal"), Player.Instance);
                    Chat.Print("Trigering heal..", System.Drawing.Color.LightGreen);
                    CastedSpells.Add("summonerHeal", perc);
                }
                
            }

            if (HasBarrier)
            {
                if (usummMenu["summonerBarrier"].Cast<Slider>().CurrentValue >= perc &&
                    !CastedSpells.Keys.Contains("summonerBarrier") &&
                    CanCast("summonerbarrier"))
                {
                    Chat.Print("Trigering barrier..", System.Drawing.Color.LightGreen);
                    Player.Instance.Spellbook.CastSpell(GetSpellSlot("summonerbarrier"), Player.Instance);
                    CastedSpells.Add("summonerBarrier", perc);
                }
            }
        }
    }
}