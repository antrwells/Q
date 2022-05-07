using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Themes
{
    public class ThemeDarkFlat : ITheme
    {

        public ThemeDarkFlat()
        {

            Button = new Texture.Texture2D("Data/UI/Theme/DarkFlatTheme/Button1.png", false);
            Frame = new Texture.Texture2D("Data/UI/Theme/DarkFlatTheme/Frame1.png", false);
            WindowTitle = new Texture.Texture2D("Data/UI/Theme/DarkFlatTheme/WindowTitle1.png", false);
            Line = new Texture.Texture2D("Data/UI/Theme/DarkFlatTheme/Line.png", false);
            FileIcon = new Texture.Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png", false);
            SystemFont = new Font.FontTTF("Data/UI/Theme/DarkFlatTheme/DarkSys.ttf", 13);
            SystemTextColor = new OpenTK.Mathematics.Vector4(0.7f, 0.7f, 0.7f, 1);
        }

    }
}
