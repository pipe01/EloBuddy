using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System.Drawing;
using Rectangle = SharpDX.Rectangle;
using Point = System.Drawing.Point;

namespace HaxorBuddy.UI
{
    class Window
    {
        public Rectangle Bounds { get; set; }
        public string Title { get; set; }
        public Font TitleFont;
        public Color BorderColor = Color.FromArgb(135, 124, 78);
        
        private Text titleText;

        public Window()
        {
            if (TitleFont == null)
            {
                TitleFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            }

            titleText = new Text(string.Empty, TitleFont);
        }

        public void Draw()
        {
            DrawRectangle(Bounds.X, Bounds.Y - 12, Bounds.Width, 12, BorderColor); // Title bar
            DrawRectangle(Bounds, BorderColor); // Control container
            
        }

        private Vector2 PointToVector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        private Vector2 PointToVector2(SharpDX.Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        private void DrawRectangle(Rectangle rect, Color borderColor)
        {
            Line.DrawLine(borderColor,
                PointToVector2(rect.Location),
                new Vector2(rect.X, rect.Y + rect.Height),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Width),
                new Vector2(rect.X + rect.Width, rect.Y));
        }
        private void DrawRectangle(int x, int y, int width, int height, Color borderColor)
        {
            Line.DrawLine(borderColor,
                new Vector2(x, y),
                new Vector2(x, y + height),
                new Vector2(x + width, y + width),
                new Vector2(x + width, y));
        }
    }
}
