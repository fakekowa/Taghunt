using Microsoft.Maui.Graphics;

namespace TagHunt.Services.Interfaces
{
    /// <summary>
    /// Interface for safe area service that provides platform-specific safe area insets and padding
    /// </summary>
    public interface ISafeAreaService
    {
        /// <summary>
        /// Gets the safe area insets for the current platform
        /// </summary>
        /// <returns>Safe area insets as Thickness</returns>
        Thickness GetSafeAreaInsets();
        
        /// <summary>
        /// Gets dynamic padding that respects safe areas while maintaining minimum values
        /// </summary>
        /// <param name="minimumHorizontal">Minimum horizontal padding (default: 20)</param>
        /// <param name="minimumVertical">Minimum vertical padding (default: 20)</param>
        /// <returns>Dynamic padding as Thickness</returns>
        Thickness GetDynamicPadding(double minimumHorizontal = 20, double minimumVertical = 20);
    }
} 