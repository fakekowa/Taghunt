using CommunityToolkit.Mvvm.ComponentModel;

namespace TagHunt.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
} 