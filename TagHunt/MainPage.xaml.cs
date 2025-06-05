namespace TagHunt;

/// <summary>
/// Main page of the application with a simple counter demonstration
/// </summary>
public partial class MainPage : ContentPage
{
	/// <summary>
	/// Counter to track the number of button clicks
	/// </summary>
	int count = 0;

	/// <summary>
	/// Initializes the main page and its components
	/// </summary>
	public MainPage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles the counter button click event and updates the display text
	/// </summary>
	/// <param name="sender">The button that was clicked</param>
	/// <param name="e">Event arguments</param>
	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
		{
			CounterBtn.Text = $"Clicked {count} time";
		}
		else
		{
			CounterBtn.Text = $"Clicked {count} times";
		}

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

