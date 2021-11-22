using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TidRod.ViewModels.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterationPage : ContentPage
    {
        private readonly RegisterationViewModel _baseView;

        public RegisterationPage()
        {
            InitializeComponent();
            BindingContext = _baseView = new RegisterationViewModel();
        }
    }
}
