namespace MastodonMaui.Controls;

public partial class SidebarButton : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text", typeof(string), typeof(SidebarButton), propertyChanged: TextChanged);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        SidebarButton control = bindable as SidebarButton;
        if (control != null)
        {
            control.TextLabel.Text = (string)newValue;
        }
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
            "Source", typeof(ImageSource), typeof(SidebarButton), propertyChanged: ImageSourceChanged);

    public ImageSource Source
    {
        get { return (ImageSource)GetValue(SourceProperty); }
        set { SetValue(SourceProperty, value); }
    }

    private static void ImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        SidebarButton control = bindable as SidebarButton;
        if (control != null)
        {
            control.Icon.Source = (ImageSource)newValue;
        }
    }

    public SidebarButton()
	{
		InitializeComponent();
	}

    private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
    {
        BasePlate.BackgroundColor = Color.FromArgb("#383838");
    }

    private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {
        BasePlate.BackgroundColor = Colors.Transparent;
    }
}