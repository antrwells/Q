using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Q.Input
{
    public class AppInput
    {

        public static Vector2 MousePosition
        {
            get;
            set;
        }

        public static bool[] MouseButton
        {
            get;
            set;
        }

        public static event Action<Keys> OnKeyDown;
        public static event Action<Keys> OnKeyUp;

        public static void KeyDown(Keys key)
        {
            OnKeyDown(key);
        }

        public static void KeyUp(Keys key)
        {
            OnKeyUp(key);
        }

    }
}
