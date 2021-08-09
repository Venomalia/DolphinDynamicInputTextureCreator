using System;

namespace DolphinDynamicInputTextureCreator.Data
{
    public class RectRegion : Other.PropertyChangedBase
    {
        public static int DecimalPlaces { get; set; } = 0;
        public static double GridX { get; set; } = 0;
        public static double GridY { get; set; } = 0;
        public static double GridHeight { get; set; } = 1;
        public static double GridWidth { get; set; } = 1;

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
                if (GridWidth > 1 & DecimalPlaces == 0)
                    value = Math.Round(value / GridWidth, MidpointRounding.ToZero) * GridWidth;
                value += GridX;
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
                if (GridHeight > 1 & DecimalPlaces == 0)
                    value = Math.Round(value / GridHeight, MidpointRounding.ToZero) * GridHeight;
                value += GridY;
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
                if (GridHeight > 1)
                    value = Math.Round(value / GridHeight, MidpointRounding.ToEven) * GridHeight;

                _height = Math.Round(value, DecimalPlaces);

                if (OwnedTexture != null)
                {
                    if ((_height + Y) > OwnedTexture.ImageHeight)
                        _height = OwnedTexture.ImageHeight - Y;
                }
                if (_height < 1)
                    _height = GridHeight;
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
                if (GridWidth > 1)
                    value = Math.Round(value / GridWidth, MidpointRounding.ToEven) * GridWidth;
                _width = Math.Round(value, DecimalPlaces);

                if (OwnedTexture != null)
                {
                    if ((_width + X) > OwnedTexture.ImageWidth)
                        _width = OwnedTexture.ImageWidth - X;
                }
                if (_width < 1)
                    _width = GridWidth;
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
