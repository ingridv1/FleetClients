using GACore.Architecture;
using System;
using System.Globalization;
using System.Windows.Data;
using GACore;
using System.Windows.Media;

namespace FleetClients.Controls
{
    public class IsInFaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IKingpinState kingpinState = value as IKingpinState;
            return kingpinState.IsInFault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RadToDegStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double rad = (float)value;
            return rad.RadToDeg();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException("RadToDegStringConverter ConvertBack()");
    }

    public class KingpinStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IKingpinState kingpinState = value as IKingpinState;
            return kingpinState.IsVirtual ? Brushes.Cyan : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException("KingpinStateColorConverter ConvertBack()");
    }
}