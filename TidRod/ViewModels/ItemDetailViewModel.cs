using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TidRod.Models;
using Xamarin.Forms;

namespace TidRod.ViewModels
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string CarId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadCarId(value);
            }
        }

        public async void LoadCarId(string itemId)
        {
            // try
            // {
            //     var item = await DataStore.GetCarAsync(itemId);
            //     Id = item.Id;
            //     Text = item.Text;
            //     Description = item.Description;
            // }
            // catch (Exception)
            // {
            //     Debug.WriteLine("Failed to Load Car");
            // }
        }
    }
}
