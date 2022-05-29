using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Themes
{
    public class ThemeDark : ITheme
    {

        public ThemeDark()
        {

            Button = new Texture.Texture2D("Data/UI/Theme/DarkTheme/Button1.png",false);
            Frame = new Texture.Texture2D("Data/UI/Theme/DarkTheme/Frame1.png", false);
            WindowTitle = new Texture.Texture2D("Data/UI/Theme/DarkTheme/WindowTitle1.png", false);
            Line = new Texture.Texture2D("Data/UI/Theme/DarkTheme/Line.png", false);
            SystemFont = new Font.FontTTF("Data/UI/Theme/DarkTheme/DarkSys.ttf", 17);
            FrameRounded = new Texture.Texture2D("Data/UI/Theme/DarkTheme/FrameRounded.png", false);
            Dragger = new Texture.Texture2D("Data/UI/Theme/DarkTheme/Dragger1.png", false);
            SystemTextColor = new OpenTK.Mathematics.Vector4(0.65f, 0.65f, 0.65f, 1.0f);
        }

    }
}
