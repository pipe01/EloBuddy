﻿using System;
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

        public static Dictionary<string, Mode> Modes = new Dictionary<string, Mode>();

        public void Init()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private void Loading_OnLoadingComplete(EventArgs args)
        {
            var cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[HaxorBuddy] Creating Menus");
            HaxorMenu.Init();

            var modes = Mode.GetAllModes();
            foreach (var item in modes)
            {
                Mode instance = (Mode)Activator.CreateInstance(item.Value);
                instance.CreateMenu();
                if (HaxorMenu.modesMenu[item.Key].Cast<CheckBox>().CurrentValue)
                {
                    instance.Init();
                    Console.WriteLine("[HaxorBuddy] [Modes] Created " + item.Key);
                }
                else
                {
                    Console.WriteLine("[HaxorBuddy] [Modes] Skipped " + item.Key);
                }
                Modes.Add(item.Key, instance);
            }

            Console.ForegroundColor = cc;

            

            Chat.Print("HaxorBuddy v1.1.0.0 init");
        }

        public static Mode GetMode(string id)
        {
            foreach (var item in Modes)
            {
                if (item.Key == id)
                    return item.Value;
            }
            return null;
        }
    }
}
