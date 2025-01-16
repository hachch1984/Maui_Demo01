using FrontEndMovile.ViewModel;

namespace FrontEndMovile.View.User;

public partial class Login_Page : ContentPage
{
	public Login_Page(Login_ViewModel login_ViewModel)
	{
		InitializeComponent();
        BindingContext = login_ViewModel;
    }
}