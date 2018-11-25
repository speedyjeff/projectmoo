using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public class OnScreenText : EphemerialElement
    {
        public string Text { get; set; }

        public OnScreenText()
        {
            Duration = 30;
        }

        public override void Draw(IGraphics g)
        {
            g.DisableTranslation();
            {
                g.Text(RGBA.Black, (g.Width/3) - (Text.Length), 10, Text);
            }
            g.EnableTranslation();
            base.Draw(g);
        }
    }
}
