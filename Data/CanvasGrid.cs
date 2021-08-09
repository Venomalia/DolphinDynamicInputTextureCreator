using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DolphinDynamicInputTextureCreator.Data
{
    /// <summary>Class <c>CanvasGrid</c> 
    /// contains the logic for the grid.
    /// </summary>
    public sealed class CanvasGrid : Other.PropertyChangedBase
    {
        private readonly double _max_width = 256;
        private readonly double _max_height = 256;

        /// <summary>
        /// specifies the start point X of the grid.
        /// </summary>
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

                RectRegion.Grid = this;
                OnPropertyChanged(nameof(X));
            }
        }

        /// <summary>
        /// specifies the start point Y of the grid.
        /// </summary>
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

                RectRegion.Grid = this;
                OnPropertyChanged(nameof(Y));
            }
        }

        /// <summary>
        /// sindicates the width of the grid.
        /// </summary>
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

                RectRegion.Grid = this;
                OnPropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// sindicates the height of the grid.
        /// </summary>
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

                RectRegion.Grid = this;
                OnPropertyChanged(nameof(Height));
            }
        }

        /// <summary>
        /// Base constructor.
        /// </summary>
        public CanvasGrid(double x, double y, double width, double height)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        /// <summary>
        /// implicit convertirung to rect.
        /// </summary>
        public static implicit operator Rect(CanvasGrid v)
        {
            return new Rect(v.X, v.Y, v.Width, v.Height);
        }

        /// <summary>
        /// implicit conversion from rect.
        /// </summary>
        public static implicit operator CanvasGrid(Rect v)
        {
            return new CanvasGrid(v.X, v.Y, v.Width, v.Height);
        }
    }
}
