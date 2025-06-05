using CommunityToolkit.Mvvm.ComponentModel;

namespace TagHunt.ViewModels;

/// <summary>
/// Base view model class that provides common properties for all view models
/// </summary>
public abstract class BaseViewModel : ObservableObject
{
    private string _title = string.Empty;
    
    /// <summary>
    /// Gets or sets the title for the view model
    /// </summary>
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _isBusy;
    
    /// <summary>
    /// Gets or sets a value indicating whether the view model is busy performing an operation
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
} 