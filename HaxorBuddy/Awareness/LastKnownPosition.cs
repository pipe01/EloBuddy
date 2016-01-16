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
        private Text ChampText, ChampTextMinimap;

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
            ChampTextMinimap = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold));

            Drawing.OnEndScene += Drawing_OnEndScene;
            Game.OnTick += Game_OnTick;

            return true;
        }

        private void Game_OnTick(EventArgs args)
        {
            foreach (var item in EntityManager.Heroes.Enemies.Where(o => !o.IsMe && !o.IsRecalling()))
            {
                if (!item.IsHPBarRendered && !Positions.ContainsKey(item.Name) && !item.IsDead)
                {
                    var posdata = new PositionData();

                    posdata.Name = item.Name;
                    posdata.ChampionName = item.ChampionName;
                    posdata.WorldPosition = item.Position;
                    posdata.MinimapPosition = Drawing.WorldToMinimap(item.Position);
                    posdata.ScreenPosition = Drawing.WorldToScreen(item.Position);
                    posdata.Angle = item.Direction.To2D();

                    Positions.Add(item.Name, posdata);
                }
                else if (item.IsHPBarRendered && Positions.ContainsKey(item.Name) || item.IsDead)
                    Positions.Remove(item.Name);
            }
        }

        private void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var item in Positions.Values)
            {
                var screenpos = Drawing.WorldToScreen(item.WorldPosition);
                var minimappos = Drawing.WorldToMinimap(Player.Instance.Position);

                ChampText.Draw(item.ChampionName, Color.Magenta, (int)screenpos.X, (int)screenpos.Y);

                ChampTextMinimap.Draw(item.ChampionName[0].ToString(), Color.Magenta,
                    (int)minimappos.X, (int)minimappos.Y);
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
        public Vector2 Angle;
    }
}
