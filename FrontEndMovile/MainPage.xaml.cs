using Dto;
using FrontEndMovile.Util;

namespace FrontEndMovile
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage(ISetting setting)
        {
            InitializeComponent();
       

            var name= Preferences.Get(nameof(Token_Dto_For_ShowInformation.Name), string.Empty);    

            this.LblWellcome.Text = $"Bienvenido usuario {name.ToUpper()} al demo de la futura aplicacion movil desarrollada en MAUI para el colegio";
        }

     
    }

}
