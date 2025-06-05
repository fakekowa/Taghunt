using TagHunt.ViewModels;

namespace TagHunt.Views;

/// <summary>
/// Account settings page where users can manage their profile information
/// </summary>
public partial class AccountSettingsPage : ContentPage
{
    /// <summary>
    /// Initializes the account settings page with dependency injection
    /// </summary>
    /// <param name="viewModel">Account settings view model instance</param>
    public AccountSettingsPage(AccountSettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 