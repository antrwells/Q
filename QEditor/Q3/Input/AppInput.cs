using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

/// <summary>
/// The Q.Input namespace is used by the Appplication to provide you with general input functionality.
/// </summary>
namespace Q.Input
{
    /// <summary>
    /// This is the class you would use to get input from the user.
    /// </summary>
    public class AppInput
    {

        /// <summary>
        /// Updated automatically by the app, this is the current mouse position in pixels.
        /// </summary>
        public static Vector2 MousePosition
        {
            get;
            set;
        }

        /// <summary>
        /// The current state of all mouse buttons. false = not pressed, true = pressed.
        /// </summary>
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
