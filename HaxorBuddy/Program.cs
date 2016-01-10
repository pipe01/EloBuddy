using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using Color = System.Drawing.Color;
using SharpDX;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX.Direct3D9;
using System.Drawing;

namespace HaxorBuddy
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Init();
        }

        public static Mode[] Modes;

        public void Init()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private void Loading_OnLoadingComplete(EventArgs args)
        {
            HaxorMenu.Init();

            Modes[0] = new Cooldown();
            Modes[0].Init();

            Modes[1] = new LineTracker();
            Modes[1].Init();

            Modes[2] = new Experience();
            Modes[2].Init();

            Modes[3] = new SpellBlocker();
            Modes[3].Init();

            Modes[4] = new AutoActivator();
            Modes[4].Init();

            Chat.Print("HaxorBuddy v1.0.0.1 init");
        }

        private bool IsModeActivated(string modeId)
        {
            return false;
        }
    }
}
