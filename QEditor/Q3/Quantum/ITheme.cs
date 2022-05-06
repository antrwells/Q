using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Texture;

namespace Q.Quantum
{
    public class ITheme
    {
        
        public Texture2D Border
        {
            get;
            set;
        }       
        
        public Texture2D Frame
        {
            get;
            set;
        }

        public Texture2D Button
        {
            get;
            set;
        }

        public Texture2D WindowTitle
        {
            get;
            set;
        }

        public Texture2D Slider
        {
            get;
            set;
        }

        public Texture2D Line
        {
            get;
            set;
        }

        public Font.FontTTF SystemFont
        {
            get;
            set;
        }

    }
}
