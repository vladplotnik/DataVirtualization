﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace VirtualCollection.Framework.Converters
{
    public class StringValueToObjectConverter : IValueConverter
    {
        private StringValueConversionCollection conversions;

        public StringValueConversionCollection Conversions
        {
            get { return conversions ?? (conversions = new StringValueConversionCollection()); }
            set { conversions = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var selector = value.ToString();

            var conversion = Conversions.FirstOrDefault(c => c.When == selector);

            return conversion != null ? conversion.Then : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
