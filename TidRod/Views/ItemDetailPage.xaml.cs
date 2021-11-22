using System.ComponentModel;
using TidRod.ViewModels;
using Xamarin.Forms;

namespace TidRod.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
