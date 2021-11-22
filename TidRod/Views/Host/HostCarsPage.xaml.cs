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
    public partial class HostCarsPage : ContentPage
    {
        private HostCarsViewModel _viewModel;

        private List<SwipeView> SwipeViews { set; get; }

        public HostCarsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HostCarsViewModel();
            SwipeViews = new List<SwipeView>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void SwipeItemSwipeStarted(
            object sender,
            SwipeStartedEventArgs e
        )
        {
            if (SwipeViews.Count == 1)
            {
                SwipeViews[0].Close();
                _ = SwipeViews.Remove(SwipeViews[0]);
            }
        }

        private void SwipeItemSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            SwipeView Swipe = sender as SwipeView;
            SwipeViews.Add (Swipe);
        }
    }
}
