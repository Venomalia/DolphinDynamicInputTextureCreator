using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DolphinDynamicInputTextureCreator.Data
{
    public sealed class CanvasGrid : Other.PropertyChangedBase
    {
        private readonly double _max_width = 256;
        private readonly double _max_height = 256;

        private double _x;
        public double X {
            get => _x;
            set
            {
                while (value >= Width)
                {
                    value -= Width;
                }
                if (value < 0)
                    value = 0;
                _x = value;

                RectRegion.GridX = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private double _y;
        public double Y {
            get => _y;
            set
            {
                while (value >= Height)
                {
                    value -= Height;
                }
                if (value < 0)
                    value = 0;
                _y = value;

                RectRegion.GridY = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                if (value > _max_width)
                    value = _max_width;
                if (value < 1)
                    value = 1;
                _width = value;

                if (X >= value) X = value - 1;

                RectRegion.GridWidth = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (value > _max_height)
                    value = _max_height;
                if (value < 1)
                    value = 1;
                _height = value;

                if (Y >= value) Y = value - 1;

                RectRegion.GridHeight = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public CanvasGrid(double x, double y, double width, double height)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public static implicit operator Rect(CanvasGrid v)
        {
            return new Rect(v.X, v.Y, v.Width, v.Height);
        }

        public static implicit operator CanvasGrid(Rect v)
        {
            return new CanvasGrid(v.X, v.Y, v.Width, v.Height);
        }
    }
}
