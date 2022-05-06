//#define Profile
using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using SharpFont.TrueType;

namespace Q.Font
{
    public class FontTTF
    {
        private static SharpFont.Library Lib = null;

        private Face _Face;

        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
                _Face.SetCharSize(0, Size, 0, 96);
            }
        }

        private int _Size = 0;

        public static void Init()
        {
            Lib = new Library();
        }

        public Dictionary<string, Texture.Texture2D> Cache = new Dictionary<string, Texture.Texture2D>();

        public FontTTF(string path, int size = 16)
        {
            _Face = new Face(Lib, path);
            Size = size;
        }

        public int Width(string text)
        {
            if (text.Length == 0) return 2;

            var str = GenString(text);
            if (str == null) return 2;
            return str.Width;
        }

        public int Height(string text = "")
        {
            return GenString("A").Height;
        }

        public Texture.Texture2D GenString(string text)
        {
            return GenString(text, Color.White, Color.Black);
        }

        public Texture.Texture2D GenString(string text, Color fore, Color bg)
        {
            if (Cache.ContainsKey(text))
            {
                if (Cache[text].Destroyed == false)
                {
                  //  Cache[text].DestoryWhen(2000);
                    return Cache[text];
                }
                else
                {
                    Cache.Remove(text);
                }
            }

            var measuredChars = new List<DebugChar>();
            var renderedChars = new List<DebugChar>();
            float penX = 0, penY = 0;
            float stringWidth = 0; // the measured width of the string
            float stringHeight = 0; // the measured height of the string
            float overrun = 0;
            float underrun = 0;
            float kern = 0;
            int spacingError = 0;
            bool trackingUnderrun = true;
            int rightEdge = 0; // tracking rendered right side for debugging

            // Bottom and top are both positive for simplicity.
            // Drawing in .Net has 0,0 at the top left corner, with positive X to the right
            // and positive Y downward.
            // Glyph metrics have an origin typically on the left side and at baseline
            // of the visual data, but can draw parts of the glyph in any quadrant, and
            // even move the origin (via kerning).
            float top = 0, bottom = 0;

            // Measure the size of the string before rendering it. We need to do this so
            // we can create the proper size of bitmap (canvas) to draw the characters on.
            for (int i = 0; i < text.Length; i++)
            {
                #region Load character

                char c = text[i];

                // Look up the glyph index for this character.
                uint glyphIndex = _Face.GetCharIndex(c);

                // Load the glyph into the font's glyph slot. There is usually only one slot in the font.
                _Face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);

                // Refer to the diagram entitled "Glyph Metrics" at http://www.freetype.org/freetype2/docs/tutorial/step2.html.
                // There is also a glyph diagram included in this example (glyph-dims.svg).
                // The metrics below are for the glyph loaded in the slot.
                float gAdvanceX = (float)_Face.Glyph.Advance.X; // same as the advance in metrics
                float gBearingX = (float)_Face.Glyph.Metrics.HorizontalBearingX;
                float gWidth = _Face.Glyph.Metrics.Width.ToSingle();
                var rc = new DebugChar(c, gAdvanceX, gBearingX, gWidth);

                #endregion Load character

                #region Underrun

                // Negative bearing would cause clipping of the first character
                // at the left boundary, if not accounted for.
                // A positive bearing would cause empty space.
                underrun += -(gBearingX);
                if (stringWidth == 0)
                    stringWidth += underrun;
                if (trackingUnderrun)
                    rc.Underrun = underrun;
                if (trackingUnderrun && underrun <= 0)
                {
                    underrun = 0;
                    trackingUnderrun = false;
                }

                #endregion Underrun

                #region Overrun

                // Accumulate overrun, which coould cause clipping at the right side of characters near
                // the end of the string (typically affects fonts with slanted characters)
                if (gBearingX + gWidth > 0 || gAdvanceX > 0)
                {
                    overrun -= Math.Max(gBearingX + gWidth, gAdvanceX);
                    if (overrun <= 0) overrun = 0;
                }
                overrun += (float)(gBearingX == 0 && gWidth == 0 ? 0 : gBearingX + gWidth - gAdvanceX);
                // On the last character, apply whatever overrun we have to the overall width.
                // Positive overrun prevents clipping, negative overrun prevents extra space.
                if (i == text.Length - 1)
                    stringWidth += overrun;
                rc.Overrun = overrun; // accumulating (per above)

                #endregion Overrun

                #region Top/Bottom

                // If this character goes higher or lower than any previous character, adjust
                // the overall height of the bitmap.
                float glyphTop = (float)_Face.Glyph.Metrics.HorizontalBearingY;
                float glyphBottom = (float)(_Face.Glyph.Metrics.Height - _Face.Glyph.Metrics.HorizontalBearingY);
                if (glyphTop > top)
                    top = glyphTop;
                if (glyphBottom > bottom)
                    bottom = glyphBottom;

                #endregion Top/Bottom

                // Accumulate the distance between the origin of each character (simple width).
                stringWidth += gAdvanceX;
                rc.RightEdge = stringWidth;
                measuredChars.Add(rc);

                #region Kerning (for NEXT character)

                // Calculate kern for the NEXT character (if any)
                // The kern value adjusts the origin of the next character (positive or negative).
                if (_Face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    kern = (float)_Face.GetKerning(glyphIndex, _Face.GetCharIndex(cNext), KerningMode.Default).X;
                    // sanity check for some fonts that have kern way out of whack
                    if (kern > gAdvanceX * 5 || kern < -(gAdvanceX * 5))
                        kern = 0;
                    rc.Kern = kern;
                    stringWidth += kern;
                }

                #endregion Kerning (for NEXT character)
            }

            stringHeight = top + bottom;

            // If any dimension is 0, we can't create a bitmap
            if (stringWidth == 0 || stringHeight == 0)
                return null;

            // Create a new bitmap that fits the string.
            //Bitmap bmp = new Bitmap((int)Math.Ceiling(stringWidth), (int)Math.Ceiling(stringHeight));
            Pixels.PixelMap pmp = new Pixels.PixelMap((int)Math.Ceiling(stringWidth), (int)Math.Ceiling(stringHeight), true);

            trackingUnderrun = true;
            underrun = 0;
            overrun = 0;
            stringWidth = 0;

            // Draw the string into the bitmap.
            // A lot of this is a repeat of the measuring steps, but this time we have
            // an actual bitmap to work with (both canvas and bitmaps in the glyph slot).
            for (int i = 0; i < text.Length; i++)
            {
                #region Load character

                char c = text[i];

                // Same as when we were measuring, except RenderGlyph() causes the glyph data
                // to be converted to a bitmap.
                uint glyphIndex = _Face.GetCharIndex(c);
                _Face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
                _Face.Glyph.RenderGlyph(RenderMode.Normal);
                FTBitmap ftbmp = _Face.Glyph.Bitmap;

                float gAdvanceX = (float)_Face.Glyph.Advance.X;
                float gBearingX = (float)_Face.Glyph.Metrics.HorizontalBearingX;
                float gWidth = (float)_Face.Glyph.Metrics.Width;

                var rc = new DebugChar(c, gAdvanceX, gBearingX, gWidth);

                #endregion Load character

                #region Underrun

                // Underrun
                underrun += -(gBearingX);
                if (penX == 0)
                    penX += underrun;
                if (trackingUnderrun)
                    rc.Underrun = underrun;
                if (trackingUnderrun && underrun <= 0)
                {
                    underrun = 0;
                    trackingUnderrun = false;
                }

                #endregion Underrun

                #region Draw glyph

                // Whitespace characters sometimes have a bitmap of zero size, but a non-zero advance.
                // We can't draw a 0-size bitmap, but the pen position will still get advanced (below).
                if ((ftbmp.Width > 0 && ftbmp.Rows > 0))
                {
                    var bd = ftbmp.BufferData;
                    var w = ftbmp.Width;
                    Bitmap cBmp = new Bitmap(ftbmp.Width, ftbmp.Rows);

                    //var h = ftbmp.h
                    for (int py = 0; py < ftbmp.Rows; py++)
                    {
                        for (int px = 0; px < ftbmp.Pitch; px++)
                        {
                            int pc = py * ftbmp.Pitch;
                            pc = pc + px;

                            int cc = bd[pc];

                            int a = 0;

                            if (cc > 5)
                            {
                                a = (int)(((float)cc / 255.0f) * 255.0f);
                            }
                            else
                            {
                                a = 0;
                            }

                            Color pix = Color.FromArgb(a, cc, cc, cc);
                            cBmp.SetPixel(px, py, pix);
                        }
                    }

                    // Get a bitmap that .Net can draw (GDI+ in this case).
                    //Bitmap cBmp = new Bitmap(64,64);//;  ftbmp. ToGdipBitmap(fore);
                    rc.Width = cBmp.Width;

                    rc.BearingX = _Face.Glyph.BitmapLeft;
                    int x = (int)Math.Round(penX + _Face.Glyph.BitmapLeft);
                    int y = (int)Math.Round(penY + top - (float)_Face.Glyph.Metrics.HorizontalBearingY);
                    //Not using g.DrawImage because some characters come out blurry/clipped. (Is this still true?)
                    for (int yr = 0; yr < cBmp.Height; yr++)
                    {
                        for (int xr = 0; xr < cBmp.Width; xr++)
                        {
                            byte r, g, b, a;
                            var opix = cBmp.GetPixel(xr, yr);
                            r = opix.R;
                            g = opix.G;
                            b = opix.B;
                            a = opix.A;

                            pmp.SetRGB(x + xr, y + yr, r, g, b, a);
                        }
                    }

                    //    g.DrawImageUnscaled(cBmp, x, y);

                    rc.Overrun = _Face.Glyph.BitmapLeft + cBmp.Width - gAdvanceX;
                    // Check if we are aligned properly on the right edge (for debugging)
                    rightEdge = Math.Max(rightEdge, x + cBmp.Width);
                    spacingError = pmp.Width - rightEdge;
                }
                else
                {
                    rightEdge = (int)(penX + gAdvanceX);
                    spacingError = pmp.Width - rightEdge;
                }

                #endregion Draw glyph

                #region Overrun

                if (gBearingX + gWidth > 0 || gAdvanceX > 0)
                {
                    overrun -= Math.Max(gBearingX + gWidth, gAdvanceX);
                    if (overrun <= 0) overrun = 0;
                }
                overrun += (float)(gBearingX == 0 && gWidth == 0 ? 0 : gBearingX + gWidth - gAdvanceX);
                if (i == text.Length - 1) penX += overrun;
                rc.Overrun = overrun;

                #endregion Overrun

                // Advance pen positions for drawing the next character.
                penX += (float)_Face.Glyph.Advance.X; // same as Metrics.HorizontalAdvance?
                penY += (float)_Face.Glyph.Advance.Y;

                rc.RightEdge = penX;
                spacingError = pmp.Width - (int)Math.Round(rc.RightEdge);
                renderedChars.Add(rc);

                #region Kerning (for NEXT character)

                // Adjust for kerning between this character and the next.
                if (_Face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    kern = (float)_Face.GetKerning(glyphIndex, _Face.GetCharIndex(cNext), KerningMode.Default).X;
                    if (kern > gAdvanceX * 5 || kern < -(gAdvanceX * 5))
                        kern = 0;
                    rc.Kern = kern;
                    penX += (float)kern;
                }

                #endregion Kerning (for NEXT character)
            }

            bool printedHeader = false;
            if (spacingError != 0)
            {
                for (int i = 0; i < renderedChars.Count; i++)
                {
                    //if (measuredChars[i].RightEdge != renderedChars[i].RightEdge)
                    //{
                    if (!printedHeader)
                        DebugChar.PrintHeader();
                    printedHeader = true;
                    // Debug.Print(measuredChars[i].ToString());
                    // Debug.Print(renderedChars[i].ToString());

                    //}
                }
                string msg = string.Format("Right edge: {0,3} ({1}) {2}",
                    spacingError,
                    spacingError == 0 ? "perfect" : spacingError > 0 ? "space  " : "clipped",
                    _Face.FamilyName);
                System.Diagnostics.Debug.Print(msg);
                //throw new ApplicationException(msg);
            }

            int loc = 0;

            Texture.Texture2D new_Tex = new Texture.Texture2D(pmp);
         

            Cache.Add(text, new_Tex);
            new_Tex.DestoryWhen(1500);

            return new_Tex;
        }
    }

    public class DebugChar
    {
        public char Char { get; set; }
        public float AdvanceX { get; set; }
        public float BearingX { get; set; }
        public float Width { get; set; }
        public float Underrun { get; set; }
        public float Overrun { get; set; }
        public float Kern { get; set; }
        public float RightEdge { get; set; }

        internal DebugChar(char c, float advanceX, float bearingX, float width)
        {
            this.Char = c; this.AdvanceX = advanceX; this.BearingX = bearingX; this.Width = width;
        }

        public override string ToString()
        {
            return string.Format("'{0}' {1,5:F0} {2,5:F0} {3,5:F0} {4,5:F0} {5,5:F0} {6,5:F0} {7,5:F0}",
                this.Char, this.AdvanceX, this.BearingX, this.Width, this.Underrun, this.Overrun,
                this.Kern, this.RightEdge);
        }

        public static void PrintHeader()
        {
            //  Debug.Print("    {1,5} {2,5} {3,5} {4,5} {5,5} {6,5} {7,5}",
            //   "", "adv", "bearing", "wid", "undrn", "ovrrn", "kern", "redge");
        }
    }
}