using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
