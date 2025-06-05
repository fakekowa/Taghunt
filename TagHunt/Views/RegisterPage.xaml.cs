using TagHunt.ViewModels;

namespace TagHunt.Views;

/// <summary>
/// Registration page that allows new users to create an account
/// </summary>
public partial class RegisterPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the RegisterPage with the specified view model
    /// </summary>
    /// <param name="viewModel">The authentication view model to bind to this page</param>
    public RegisterPage(AuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 