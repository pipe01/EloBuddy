using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiSinged
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Init();
        }

        Menu menu;
        Menu cmenu;

        public void Init()
        {
            Game.OnUpdate += Game_OnUpdate;
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private void Loading_OnLoadingComplete(EventArgs args)
        {
            menu = MainMenu.AddMenu("PiSinged", "mainmenu");
            cmenu = menu.AddSubMenu("Combo");

            cmenu.AddGroupLabel("Combo configuration");
            cmenu.Add("kbEWCombo", new KeyBind("E+W Combo", false, KeyBind.BindTypes.HoldActive, 'T'));

            ComboManager.Init();
        }

        private void Game_OnUpdate(EventArgs args)
        {
            var useT = cmenu["kbEWCombo"].Cast<KeyBind>().CurrentValue;

            if (useT)
            {
                var target = EntityManager.Heroes.Enemies.FirstOrDefault
                    (o => o.Position.IsInRange(Game.CursorPos, 250));
                if (target != null)
                {
                    ComboManager.Combo("ew", target);
                }
            }
        }
    }
}
