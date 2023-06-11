using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SolderingStationClient.Presentation.Helpers;

public class ExceptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        List<string> errors = new();
        if (value is ICollection list)
        {
            foreach (var item in list)
            {
                if (item is Exception e)
                    errors.Add(e.Message);
                else
                {
                    errors.Add(item.ToString());
                }
            }

            return errors;
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}