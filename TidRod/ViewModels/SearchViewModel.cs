using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TidRod.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TidRod.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private Car _selectedCar;

        public ObservableCollection<Car> Cars { get; }

        public Command LoadCarsCommand { get; }

        public SearchViewModel()
        {
            Title = "Cars Around You";

            Cars = new ObservableCollection<Car>();
            OpenWebCommand =
                new Command(async () =>
                        await Browser
                            .OpenAsync("https://aka.ms/xamarin-quickstart"));
            LoadCarsCommand =
                new Command(async () => await GetCarsAroundUserCommand());
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedCar = null;
        }

        public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                SetProperty(ref _selectedCar, value);
                OnCarSelected(value);
            }
        }

        async void OnCarSelected(Car item)
        {
            if (item == null)
                return;

            // This will push the CarDetailPage onto the navigation stack
            // await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.CarId)}={item.Id}");
        }

        public ICommand OpenWebCommand { get; }

        public async Task GetCarsAroundUserCommand()
        {
            IsBusy = true;

            try
            {
                Cars.Clear();
                var items = await DataStore.GetCarsAsync(true);
                foreach (var item in items)
                {
                    Cars.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Command loadItemsCommand;

        public ICommand LoadItemsCommand
        {
            get
            {
                if (loadItemsCommand == null)
                {
                    loadItemsCommand = new Command(LoadItems);
                }

                return loadItemsCommand;
            }
        }

        private void LoadItems()
        {
        }
    }
}
