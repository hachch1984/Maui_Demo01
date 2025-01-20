namespace FrontEndMovile
{
    public partial class App : Application
    {
        private readonly AppShell appShell;

        public App(AppShell appShell )
        {
            InitializeComponent();
            this.appShell = appShell;
        }

       
        protected override Window CreateWindow(IActivationState? activationState)
        {
           return new Window(this.appShell);
           //return new Window(appShell);

           // var accessToken = Preferences.Get(nameof(Token_Dto_For_ShowInformation.Token), string.Empty);

           // if (string.IsNullOrEmpty(accessToken))
           // {
           //     return new Window(login_Page);
           // }
           // else
           // {
           //     //
               
           // }
        }
    }
}