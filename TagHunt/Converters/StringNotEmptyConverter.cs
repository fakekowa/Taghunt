using System.Globalization;

namespace TagHunt.Converters
{
    /// <summary>
    /// Converter that returns true if string is not null or empty, false otherwise
    /// Used for visibility binding based on whether a string has content
    /// </summary>
    public class StringNotEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string value to a boolean indicating if it's not empty
        /// </summary>
        /// <param name="value">The string value to check</param>
        /// <param name="targetType">The target type (boolean)</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>True if string is not null or empty, false otherwise</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value?.ToString());
        }

        /// <summary>
        /// Converts back from boolean to string (not implemented)
        /// </summary>
        /// <param name="value">The boolean value</param>
        /// <param name="targetType">The target type (string)</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture information</param>
        /// <returns>NotImplementedException</returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringNotEmptyConverter does not support ConvertBack");
        }
    }
} 