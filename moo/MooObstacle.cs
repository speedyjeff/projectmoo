using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooObstacle : Obstacle
    {
        public MooObstacle()
        {
            Height = Width = 50;  // hit box
            ImgHeight = ImgWidth = 100;
            TakesDamage = true;
            Health = Int32.MaxValue;
            ShowDamage = false;
        }

        private int ImgHeight;
        private int ImgWidth;

        public override string ImagePath => @"media\food.png";

        public override void Draw(IGraphics g)
        {
            if (Image == null) Image = g.CreateImage(ImagePath);
            g.Image(Image, X - (ImgWidth / 2), Y - (ImgHeight / 2), ImgWidth, ImgHeight);

            base.Draw(g);
        }

        #region private
        private IImage Image;
        #endregion
    }
}
