using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace DolphinDynamicInputTextureCreator.ValueConverters
{

    [ValueConversion(typeof(int), typeof(int))]
    public class IntWithinMinAndMaxConverter : WithinMinAndMaxConverter<int> {}

    public class WithinMinAndMaxConverter<T> : IValueConverter where T : IComparable<T>, IConvertible
    {
        public T Min { get; set; }
        public T Max { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Within((T)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Within((T)System.Convert.ChangeType(value, typeof(T)));
            }
            catch(Exception)
            {
                return Min;
            }
        }

        private T Within(T value)
        {
            if (value.CompareTo(Min) < 0)
            {
                return Min;
            }
            if (value.CompareTo(Max) > 0)
            {
                return Max;
            }
            return value;
        }

    }
}
