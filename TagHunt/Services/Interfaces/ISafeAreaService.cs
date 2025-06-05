using Microsoft.Maui.Graphics;

namespace TagHunt.Services.Interfaces
{
    public interface ISafeAreaService
    {
        Thickness GetSafeAreaInsets();
        Thickness GetDynamicPadding(double minimumHorizontal = 20, double minimumVertical = 20);
    }
} 