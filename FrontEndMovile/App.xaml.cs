using Dto;
using FrontEndMovile.View.User;

namespace FrontEndMovile
{
    public partial class App : Application
    {
        private readonly Login_Page login_Page;
        private readonly AppShell appShell;

        public App(Login_Page login_Page, AppShell appShell)
        {
            InitializeComponent();
            this.login_Page = login_Page;
            this.appShell = appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //    var userId = Preferences.Get(nameof(Dto.Token_Dto_For_ShowInformation.Id), string.Empty);

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