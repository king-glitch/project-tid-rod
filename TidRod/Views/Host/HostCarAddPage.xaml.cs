using System;
using TidRod.Models;
using TidRod.ViewModels.Host;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Host
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostCarAddPage : ContentPage
    {
        private HostCarAddViewModel _viewModel;

        public HostCarAddPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HostCarAddViewModel();
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
                _viewModel.Gear =
                    (
                    CarTransmission
                    )(CarGear.ItemsSource[CarGear.SelectedIndex].ToString() ==
                    "Automatic"
                        ? 0
                        : 1);
            }
        }
    }
}
