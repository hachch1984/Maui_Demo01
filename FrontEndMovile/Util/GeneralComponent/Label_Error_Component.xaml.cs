namespace FrontEndMovile.Util.GeneralComponent;

public partial class Label_Error_Component : ContentView
{
    public Label_Error_Component()
    {
        InitializeComponent();

        // Configura el enlace de Text
        ErrorLabelControl.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(Label_Error_Component), string.Empty,
            propertyChanged: OnTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Label_Error_Component)bindable;
        var text = newValue as string;

        // Actualiza la visibilidad en base al texto
        control.ErrorLabelControl.IsVisible = !string.IsNullOrEmpty(text);
    }
}
