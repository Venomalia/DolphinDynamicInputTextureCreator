﻿using System;

namespace DolphinDynamicInputTextureCreator.Data
{
    public class RectRegion : Other.PropertyChangedBase
    {
        public static int DecimalPlaces { get; set; } = 0;
        public static int GridHeight { get; set; } = 1;
        public static int GridWidth { get; set; } = 1;

        private EmulatedDevice _emulated_device;
        public EmulatedDevice Device
        {
            get { return _emulated_device; }
            set
            {
                _emulated_device = value;
                OnPropertyChanged(nameof(Device));
            }
        }

        private EmulatedKey _emulated_key;
        public EmulatedKey Key
        {
            get { return _emulated_key; }
            set
            {
                _emulated_key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        private DynamicInputTexture _owned_texture;
        public DynamicInputTexture OwnedTexture
        {
            get { return _owned_texture; }
            set
            {
                _owned_texture = value;
                OnPropertyChanged(nameof(OwnedTexture));
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Width));
            }
        }

        private double _scale_factor;
        public double ScaleFactor
        {
            get { return _scale_factor; }
            set
            {
                _scale_factor = value;
                OnPropertyChanged(nameof(ScaleFactor));
                OnPropertyChanged(nameof(ScaledX));
                OnPropertyChanged(nameof(ScaledY));
                OnPropertyChanged(nameof(ScaledHeight));
                OnPropertyChanged(nameof(ScaledWidth));
            }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _x = Math.Round(value / GridWidth, DecimalPlaces) * GridWidth;
                if (_x < 0)
                    _x = 0;

                if (OwnedTexture != null)
                {
                    if (_x + Width > OwnedTexture.ImageWidth)
                        _x = OwnedTexture.ImageWidth - Width;
                }

                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(ScaledX));
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                _y = Math.Round(value / GridHeight, DecimalPlaces) * GridHeight;
                if (_y < 0)
                    _y = 0;

                if (OwnedTexture != null)
                {
                    if (_y + Height > OwnedTexture.ImageWidth)
                        _y = OwnedTexture.ImageWidth - Height;
                }

                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(ScaledY));
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = Math.Round(value / GridHeight, DecimalPlaces) * GridHeight;
                if (OwnedTexture != null)
                {
                    if ((_height + Y) > OwnedTexture.ImageHeight)
                        _height = OwnedTexture.ImageHeight - Y;
                }
                if (_height < 1)
                    _height = 1;
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(ScaledHeight));
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                _width = Math.Round(value / GridWidth, DecimalPlaces) * GridWidth;
                if (OwnedTexture != null)
                {
                    if ((_width + X) > OwnedTexture.ImageWidth)
                        _width = OwnedTexture.ImageWidth - X;
                }
                if (_width < 1)
                    _width = 1;
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(ScaledWidth));
            }
        }

        public double ScaledX
        {
            get
            {
                return X * ScaleFactor;
            }
            set
            {
                X = value / ScaleFactor;
            }
        }
        public double ScaledY
        {
            get
            {
                return Y * ScaleFactor;
            }
            set
            {
                Y = value / ScaleFactor;
            }
        }
        public double ScaledWidth
        {
            get 
            {
                return Width * ScaleFactor;
            }
            set
            {
                Width = value / ScaleFactor;
            }
        }

        public double ScaledHeight
        {
            get
            {
                return Height * ScaleFactor;
            }
            set
            {
                Height = value / ScaleFactor;
            }
        }
    }
}
