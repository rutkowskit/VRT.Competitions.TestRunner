using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace VRT.Competitions.TestRunner.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Services.GetRequiredService<MainWindowViewModel>();
        DataContext = ViewModel;
    }
    private MainWindowViewModel ViewModel { get; }

    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is not null)
        {
            grid.ScrollIntoView(grid.SelectedItem);
        }
    }

    private void OnBrowseExeFileButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "exe|*.exe",
            Title = "Select executable to test",
            Multiselect = false            
        };
        if(dialog.ShowDialog(this) is false)
        {
            return;
        }
        ViewModel.ExecutableFilePath = dialog.FileName;
    }

    private void OnBrowseTestFilesFolderButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new BrowseForFolder();        
        var wih = new System.Windows.Interop.WindowInteropHelper(this);
        var initialDir = ViewModel.TestsDirectoryPath;
        dialog.SelectFolder("Select folder containing *.in and *.out files", initialDir, wih.Handle);
    }
}
