using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace HaxorBuddy.UI
{
    class MouseHandler
    {
        public static MouseHandler Instance = new MouseHandler();

        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONDC = 0x203;
        private const int WM_MOUSEMOVE = 0x200;

        public enum MouseEventType
        {
            LeftButtonDown,
            LeftButtonUp,
            LeftButtonDoubleClick,
            MouseMove
        }

        public MouseHandler()
        {
            Game.OnWndProc += Game_OnWndProc;
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            switch (args.Msg)
            {
                case WM_LBUTTONDOWN:
                    OnMouseEventI(MouseEventType.LeftButtonDown, Game.CursorPos2D);
                    break;
                case WM_LBUTTONUP:
                    OnMouseEventI(MouseEventType.LeftButtonUp, Game.CursorPos2D);
                    break;
                case WM_LBUTTONDC:
                    OnMouseEventI(MouseEventType.LeftButtonDoubleClick, Game.CursorPos2D);
                    break;
                case WM_MOUSEMOVE:
                    OnMouseEventI(MouseEventType.MouseMove, Game.CursorPos2D);
                    break;
            }
        }

        public delegate void MouseEvent(MouseEventType type, Vector2 pos);
        public event MouseEvent OnMouseEvent;
        private void OnMouseEventI(MouseEventType type, Vector2 pos)
        {
            if (OnMouseEvent != null)
                OnMouseEvent(type, pos);
        }
    }
}
