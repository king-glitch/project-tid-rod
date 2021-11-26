using System;
using TidRod.Models;
using TidRod.ViewModels.Host;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Host
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostCarUpdatePage : ContentPage
    {
        private HostCarUpdateViewModel _viewModel;

        public HostCarUpdatePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HostCarUpdateViewModel();
        }

        private void SelectGearPickerSelectedIndexChanged(
            object sender,
            EventArgs e
        )
        {
            // check if car is selected;
            if (CarGear.SelectedIndex >= 0)
            {
                // transform word to car transmission enum;
                var value =
                    CarGear.ItemsSource[CarGear.SelectedIndex].ToString();
                _viewModel.Gear =
                    (CarTransmission)(value == "Automatic" ? 0 : 1);
            }
        }
    }
}
