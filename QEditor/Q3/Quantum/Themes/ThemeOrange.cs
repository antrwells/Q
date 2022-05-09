using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum;
using Q.Texture;
using Q.Font;
using Q;

namespace Q.Quantum.Themes
{
    public class ThemeOrange : ITheme
    {

        public ThemeOrange()
        {

            Button = new Texture2D("Data/UI/Theme/OrangeTheme/Button1.png", false);
            Frame = new Texture2D("Data/UI/Theme/OrangeTheme/Frame1.png", false);
            WindowTitle = new Texture2D("Data/UI/Theme/OrangeTheme/WindowTitle1.png", false);
            Line = new Texture2D("Data/UI/Theme/DarkTheme/Line.png", false);
            SystemFont = new FontTTF("Data/UI/Theme/OrangeTheme/DarkSys.ttf", 16);
            FrameRounded = new Texture2D("Data/UI/Theme/OrangeTheme/FrameRounded.png", false);
            Dragger = new Texture2D("Data/UI/Theme/OrangeTheme/Dragger1.png", false);
            SystemTextColor = new OpenTK.Mathematics.Vector4(0.85f, 0.85f, 0.85f, 1.0f);
        }

    }
}
