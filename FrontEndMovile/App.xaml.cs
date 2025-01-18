using Dto;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class App : Application
    {
        private readonly Login_Page login_Page;
        private readonly AppShell appShell;

        public App(Login_Page login_Page, AppShell appShell,PasswordRestore_Page passwordRestore_Page)
        {
            InitializeComponent();
            this.login_Page = login_Page;
            this.appShell = appShell;
            this.PasswordRestore_Page = passwordRestore_Page;

            this.login_Page.PasswordRestore_Page = this.PasswordRestore_Page;
         
            this.appShell.Login_Page = this.login_Page;
        }

        public PasswordRestore_Page PasswordRestore_Page { get; }

        protected override Window CreateWindow(IActivationState? activationState)
        {
           
          

            var accessToken = Preferences.Get(nameof(Token_Dto_For_ShowInformation.Token), string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                return new Window(login_Page);
            }
            else
            {
                //return new Window(new AppShell());
                return new Window(appShell);
            }
        }
    }
}