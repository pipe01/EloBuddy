using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace HaxorBuddy
{
    class Utils
    {
        public static AIHeroClient GetHeroFromName(string name)
        {
            return EntityManager.Heroes.AllHeroes.Find(o => o.Name == name);
        }

        public static bool IsOnScreen(Vector2 position)
        {
            return position.To3D().IsOnScreen();
        }
    }
}
