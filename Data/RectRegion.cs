using System;
using System.Windows;
﻿using Newtonsoft.Json;

namespace DolphinDynamicInputTextureCreator.Data
{
    public class RectRegion : Other.PropertyChangedBase
    {

        /// <summary>
        /// this determines the sub pixel value, 0 uses only full pixels.
        /// </summary>
        public static int DecimalPlaces { get; set; } = 0;
        public static CanvasGrid Grid { get;  set; }

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
                if (Grid.Width > 1 & DecimalPlaces == 0 & value > 0.6)
                {
                    value = Math.Round(value / Grid.Width, MidpointRounding.ToZero) * Grid.Width;
                    value += Grid.X;
                }
                _x = Math.Round(value, DecimalPlaces);

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
                if (Grid.Height > 1 & DecimalPlaces == 0 & value > 0.6)
                {
                    value = Math.Round(value / Grid.Height, MidpointRounding.ToZero) * Grid.Height;
                    value += Grid.Y;
                }
                _y = Math.Round(value, DecimalPlaces);

                if (_y < 0)
                    _y = 0;

                if (OwnedTexture != null)
                {
                    if (_y + Height > OwnedTexture.ImageHeight)
                        _y = OwnedTexture.ImageHeight - Height;
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
                if (OwnedTexture != null && value + Y <= OwnedTexture.ImageHeight)
                {
                    if (Y == 0 & Grid.Y != 0)
                        value += Grid.Y;

                    if (Grid.Height > 1)
                        value = Math.Round(value / Grid.Height, MidpointRounding.ToEven) * Grid.Height;

                    if (Y == 0 & Grid.Y != 0)
                        value -= Grid.Height - Grid.Y;
                }
                _height = Math.Round(value, DecimalPlaces);

                if (OwnedTexture != null && _height + Y > OwnedTexture.ImageHeight)
                    _height = OwnedTexture.ImageHeight - Y;

                if (_height < 0.1)
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
                if (OwnedTexture != null && value + X <= OwnedTexture.ImageWidth)
                {
                    if (X == 0 & Grid.X != 0)
                        value += Grid.X;

                    if (Grid.Width > 1)
                        value = Math.Round(value / Grid.Width, MidpointRounding.ToEven) * Grid.Width;

                    if (X == 0 & Grid.X != 0)
                        value -= Grid.Width - Grid.X;
                }
                _width = Math.Round(value, DecimalPlaces);

                if (OwnedTexture != null && _width + X > OwnedTexture.ImageWidth)
                    _width = OwnedTexture.ImageWidth - X;

                if (_width < 0.1)
                    _width = 1;

                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(ScaledWidth));
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        /// <summary>
        /// implicit convertirung to rect.
        /// </summary>
        public static implicit operator Rect(RectRegion v)
        {
            return new Rect(v.X, v.Y, v.Width, v.Height);
        }

        /// <summary>
        /// implicit conversion from rect.
        /// </summary>
        public static implicit operator RectRegion(Rect v)
        {
            return new RectRegion() { X = v.X, Y = v.Y, Width = v.Width, Height = v.Height };
        }

    }
}
