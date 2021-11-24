using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void SelectGearPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (CarGear.SelectedIndex != -1)
            {
                var value = CarGear.ItemsSource[CarGear.SelectedIndex].ToString();
                _viewModel.Gear = (CarTransmission)(value == "Automatic" ? 0 : 1);

            }
        }
    }
}
