using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace HaxorBuddy.Awareness
{
    class LastKnownPosition : Mode
    {
        private Dictionary<string, PositionData> Positions = new Dictionary<string, PositionData>();
        private Text ChampText;

        public override void CreateMenu()
        {

        }

        public override bool DefaultEnabled()
        {
            return true;
        }

        public override string GetID()
        {
            return "Last Known Position";
        }

        public override bool Init()
        {
            ChampText = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold));

            Drawing.OnEndScene += Drawing_OnEndScene;
            Game.OnTick += Game_OnTick;

            return true;
        }

        private void Game_OnTick(EventArgs args)
        {
            foreach (var item in EntityManager.Heroes.Enemies.Where(o => !o.IsMe && !o.IsRecalling()))
            {
                if (!item.IsHPBarRendered && !Positions.ContainsKey(item.Name) && !item.IsDead)
                    Positions.Add(item.Name, new PositionData()
                    {
                        Name = item.Name,
                        ChampionName = item.ChampionName,
                        WorldPosition = item.Position,
                        MinimapPosition = Drawing.WorldToMinimap(item.Position),
                        ScreenPosition = Drawing.WorldToScreen(item.Position),
                    });
                else if (item.IsHPBarRendered && Positions.ContainsKey(item.Name) || item.IsDead)
                    Positions.Remove(item.Name);
            }
        }

        private void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var item in Positions)
            {
                var screenpos = Drawing.WorldToScreen(item.Value.WorldPosition);
                var minimappos = item.Value.MinimapPosition;
                //var hero = Utils.GetHeroFromName(item.Key);

      
                ChampText.Draw(item.Value.ChampionName, Color.Magenta, (int)screenpos.X, (int)screenpos.Y);
                ChampText.Draw(item.Value.ChampionName[0].ToString(), Color.Magenta, (int)minimappos.X, (int)minimappos.Y);
                //Chat.Print(minimappos.X + " " + minimappos.Y + " " + item.Value.ChampionName);
            }
        }

        public override void Stop()
        {
            Drawing.OnEndScene -= Drawing_OnEndScene;
            Game.OnTick -= Game_OnTick;
        }
    }

    public class PositionData
    {
        public string Name, ChampionName;
        public Vector2 ScreenPosition, MinimapPosition;
        public Vector3 WorldPosition;
    }
}
