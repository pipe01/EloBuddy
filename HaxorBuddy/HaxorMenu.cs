using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxorBuddy
{
    class HaxorMenu
    {
        public static Menu haxorMenu, modesMenu;

        public static void Init()
        {
            haxorMenu = MainMenu.AddMenu("HaxorBuddy", "haxorBuddy");

            modesMenu = haxorMenu.AddSubMenu("Modes");
            
            modesMenu.AddGroupLabel("Modes");
            foreach (var item in Mode.GetAllModes())
            {
                modesMenu.Add(item.Key, new CheckBox(item.Key, false)).OnValueChange += HaxorMenu_OnValueChange;
            }
            modesMenu.AddSeparator();
            modesMenu.Add("panicbtn", new CheckBox("Panic button", false)).OnValueChange += PanicButton;
        }

        private static void PanicButton(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue)
            {
                foreach (var item in Mode.GetAllModes())
                {
                    Program.ModeStop(item.Key);
                    modesMenu[item.Key].Cast<CheckBox>().CurrentValue = false;
                }
                sender.CurrentValue = false;
            }
        }

        private static void HaxorMenu_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue == false)
                Program.ModeStop(sender.DisplayName);
            else
                Program.ModeStart(sender.DisplayName);

            RefreshModes();
            
            Console.WriteLine("[HaxorBuddy] [Modes] {0} {1}", args.NewValue ? "Enabling" : "Disabling", sender.DisplayName);
        }

        public static void RefreshModes()
        {
            foreach (var item in Program.Modes)
            {
                modesMenu[item.Key].Cast<CheckBox>().CurrentValue = Program.EnabledModes[item.Key];
            }
        }
    }
}
