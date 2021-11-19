using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Views;
using Xamarin.Forms;

namespace TidRod.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Car _selectedCar;

        public ObservableCollection<Car> Cars { get; }
        public Command LoadCarsCommand { get; }
        public Command AddCarCommand { get; }
        public Command<Car> CarTapped { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Cars = new ObservableCollection<Car>();
            LoadCarsCommand = new Command(async () => await ExecuteLoadCarsCommand());

            CarTapped = new Command<Car>(OnCarSelected);

            AddCarCommand = new Command(OnAddCar);
        }

        async Task ExecuteLoadCarsCommand()
        {
            IsBusy = true;

            try
            {
                Cars.Clear();
                var cars = await CarDataStore.GetCarsAsync(true);
                foreach (var car in cars)
                {
                    Cars.Add(car);
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

        private async void OnAddCar(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnCarSelected(Car car)
        {
            if (car == null)
                return;

            // This will push the CarDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.CarId)}={car.Id}");
        }
    }
}