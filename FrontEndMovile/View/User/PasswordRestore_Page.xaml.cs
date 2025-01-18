namespace FrontEndMovile.View.User;

public partial class PasswordRestore_Page : ContentPage
{
  

    public void Clear()
    {
        this.TxtEmail.EntryClearText();
    }

    public PasswordRestore_Page(PasswordRestore_ViewModel passwordRestore_ViewModel)
    {
        InitializeComponent();
    
        BindingContext = passwordRestore_ViewModel;
    }

   
}