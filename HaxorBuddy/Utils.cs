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

        public static float GetAngle(float x1, float x2, float y1, float y2)
        {
            float xDiff = x2 - x1;
            float yDiff = y2 - y1;
            return (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
        }

        public static Vector2 Reverse(Vector2 vec, Vector2 origin)
        {
            return new Vector2(origin.X - vec.X, origin.Y - vec.Y);
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
