using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Aurelia.DotNet;

namespace Aurelia.DotNet.Wizard
{ 
    [ValueConversion(typeof(Enum), typeof(string[]))]
    public class EnumValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value == null ? Binding.DoNothing : Enum.GetValues((Type)value);
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }

    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringValueConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>  value == null ? Binding.DoNothing : ((Enum)value).Description();
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }
}
