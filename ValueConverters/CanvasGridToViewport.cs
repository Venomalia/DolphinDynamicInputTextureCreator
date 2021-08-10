using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DolphinDynamicInputTextureCreator.ValueConverters
{
    class CanvasGridToViewport : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Rect = (Rect)(Data.CanvasGrid)value;
            Rect.Width *= 2;
            Rect.Height *= 2;
            return (Rect)Rect;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Rect = (Rect)value;
            Rect.Width *= 0.5;
            Rect.Height *= 0.5;
            return (Rect)Rect;
        }
    }
}
