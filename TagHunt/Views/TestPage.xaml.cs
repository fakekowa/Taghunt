namespace TagHunt.Views;

/// <summary>
/// Simple test page to verify navigation is working
/// </summary>
public partial class TestPage : ContentPage
{
    /// <summary>
    /// Initializes the test page
    /// </summary>
    public TestPage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Handles the go back button click
    /// </summary>
    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
} 