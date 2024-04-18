using Microsoft.UI.Xaml.Controls;

using Taskbar_Customizer.ViewModels;

namespace Taskbar_Customizer.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
