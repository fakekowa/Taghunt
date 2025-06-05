using TagHunt.ViewModels;

namespace TagHunt.Views;

/// <summary>
/// Main dashboard/home page that users see after successful login
/// Displays location sharing status, quick actions, and recent activity
/// </summary>
public partial class DashboardPage : ContentPage
{
    /// <summary>
    /// Initializes the dashboard page with dependency injection
    /// </summary>
    /// <param name="viewModel">Dashboard view model instance</param>
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 