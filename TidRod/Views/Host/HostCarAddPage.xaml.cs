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
    public partial class HostCarAddPage : ContentPage
    {
        private HostCarAddViewModel _viewModel;
        public HostCarAddPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HostCarAddViewModel();
        }
    }
}
