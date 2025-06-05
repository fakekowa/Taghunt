using TagHunt.ViewModels;

namespace TagHunt.Views;

/// <summary>
/// Login page that allows users to authenticate with the application
/// </summary>
public partial class LoginPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the LoginPage with the specified view model
    /// </summary>
    /// <param name="viewModel">The authentication view model to bind to this page</param>
    public LoginPage(AuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 