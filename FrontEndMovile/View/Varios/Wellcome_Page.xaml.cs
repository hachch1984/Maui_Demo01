using Dto.Ges.User;

namespace FrontEndMovile.View.Varios;

public partial class Wellcome_Page : ContentPage
{
	public Wellcome_Page()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
       


        base.OnAppearing();


        var name = Preferences.Get(nameof(Token_Created.Name), string.Empty);

        this.LblWellcome.Text = $"Bienvenido usuario {name.ToUpper()} al demo de la futura aplicacion movil desarrollada en MAUI para el colegio";

    }
}