using FrontEndMovile.ViewModel;

namespace FrontEndMovile.View.User;

public partial class Login_Page : ContentPage
{
    private readonly AppShell appShell;
    public PasswordRestore_Page PasswordRestore_Page { get; set; }


    public void Clear ()
    {
     this.CmbDocument.SelectedIndex = -1;
        this.TxtDocumentoIdentidad.EntryClearText();
        this.TxtPassword.EntryClearText();
    }

    public Login_Page(Login_ViewModel login_ViewModel)
    {
        InitializeComponent();
        BindingContext = login_ViewModel;

        
        var forgotPassword_TapGestureRecognizer = new TapGestureRecognizer();
        forgotPassword_TapGestureRecognizer.Tapped += (s, e) =>
        {
           this.PasswordRestore_Page.Clear();
            Application.Current.MainPage = this.PasswordRestore_Page;
        };
        this.ForgotPassword.GestureRecognizers.Add(forgotPassword_TapGestureRecognizer);
    }
}