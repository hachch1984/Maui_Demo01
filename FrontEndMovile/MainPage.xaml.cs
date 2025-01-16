using FrontEndMovile.Util;

namespace FrontEndMovile
{
    public partial class MainPage : ContentPage
    {
        private readonly ISetting setting;
        int count = 0;

        public MainPage(ISetting setting)
        {
            InitializeComponent();
            this.setting = setting;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time ";
            else
                CounterBtn.Text = $"Clicked {count} times "+this.setting.BackendApiUrl;

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
