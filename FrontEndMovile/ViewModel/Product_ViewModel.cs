using CommunityToolkit.Mvvm.ComponentModel;
using Dto.Ges.Product;
using System.Collections.ObjectModel;

namespace FrontEndMovile.ViewModel
{
    public partial class Product_ViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ShowInformation01Prod> products;

        private readonly IConnectivity connectivity;
        public Product_ViewModel(IConnectivity connectivity)
        {
            this.connectivity = connectivity;
            this.connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }



        private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool StatusConnection => connectivity.NetworkAccess == NetworkAccess.Internet;

    }
}
