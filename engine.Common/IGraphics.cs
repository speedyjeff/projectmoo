﻿using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common
{
    public struct RGBA
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public static RGBA Black = new RGBA() { R = 0, G = 0, B = 0, A = 255 };
        public static RGBA White = new RGBA() { R = 255, G = 255, B = 255, A = 255 };
    }

    public delegate bool TranslateCoordinatesDelegate(bool autoScale, float x, float y, float width, float height, float other, out float tx, out float ty, out float twidth, out float theight, out float tother);

    public interface IGraphics
    {
        // drawing
        void Clear(RGBA color);
        void Ellipse(RGBA color, float x, float y, float width, float height, bool fill = true);
        void Rectangle(RGBA color, float x, float y, float width, float height, bool fill = true);
        void Text(RGBA color, float x, float y, string text, float fontsize = 16);
        void Line(RGBA color, float x1, float y1, float x2, float y2, float thickness);
        void Image(string path, float x, float y, float width = 0, float height = 0);

        void RotateTransform(float angle);

        void DisableTranslation(bool nonScaledTranslation=false);
        void EnableTranslation();

        // details
        int Height { get; }
        int Width { get; }

        // translate the coordinates to screen
        // take into acount windowing and scalling
        void SetTranslateCoordinates(TranslateCoordinatesDelegate callback);
    }
}