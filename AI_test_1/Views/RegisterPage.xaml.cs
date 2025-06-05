using AI_test_1.ViewModels;

namespace AI_test_1.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(AuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 