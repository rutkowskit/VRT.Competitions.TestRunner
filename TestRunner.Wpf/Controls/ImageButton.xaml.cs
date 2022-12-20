using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace VRT.Competitions.TestRunner.Wpf.Controls;

public partial class ImageButton : Button
{    
    public ImageButton()
    {
        InitializeComponent();        
    }
    public static readonly DependencyProperty ButtonTextProperty
            = DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(ImageButton),
                new PropertyMetadata(null, null));
    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public static readonly DependencyProperty IconKindProperty
        = DependencyProperty.Register(nameof(IconKind), typeof(PackIconKind), typeof(ImageButton),
            new PropertyMetadata(PackIconKind.Bullet, null));
    public PackIconKind IconKind
    {
        get => (PackIconKind)GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if(e.Property.Name == nameof(ButtonText))
        {
            uxIcon.Margin = string.IsNullOrWhiteSpace(e.NewValue?.ToString())
                ? new Thickness(0)
                : new Thickness(-10, 0, 5, 0);
        }
        base.OnPropertyChanged(e);
    }
}
