using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Views.Host;
using Xamarin.Forms;

namespace TidRod.ViewModels.Host
{
    public class HostCarsViewModel : BaseViewModel
    {
        private Car _selectedCar;

        public ObservableCollection<Car> Cars { get; }
        public Command LoadCarsCommand { get; }
        public Command AddCarCommand { get; }
        public Command<Car> UpdateTapped { get; }
        public Command<Car> DeleteTapped { get; }
        public Command<Car> CarTapped { get; }
        public HostCarsViewModel()
        {
            Title = "Host Cars";
            Cars = new ObservableCollection<Car>();
            LoadCarsCommand = new Command(async () => await ExecuteLoadCarsCommand());

            CarTapped = new Command<Car>(OnCarSelected);
            AddCarCommand = new Command(OnAddCar);
            DeleteTapped = new Command<Car>(DeleteCar);
        }

        private async void DeleteCar(Car obj)
        {
            if (obj == null)
            {
                return;
            }

            bool answer = await Application.Current.MainPage.DisplayAlert("Confirm Delete Car?", obj.Name, "Yes", "No");
            if (answer)
            {
                await this.CarDataStore.DeleteCarAsync(obj.Id);
                await ExecuteLoadCarsCommand();
            }
        }

        private async Task ExecuteLoadCarsCommand()
        {
            IsBusy = true;

            try
            {
                // clear current cars;
                Cars.Clear();
                // get all user cars
                var cars = await this.UserDataStore.GetUserCarsAsync(App.CurrentSession);

                // add all cars to the collection;
                foreach (Car car in cars)
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
                // release the busy task;
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
            // goto add car page;
            await Shell.Current.GoToAsync(nameof(HostCarAddPage));
        }

        private async void OnCarSelected(Car car)
        {
            // check if the selected car is valid;
            if (car == null)
            {
                return;
            }
            // goto update car page;
            await Shell.Current.GoToAsync($"{nameof(HostCarUpdatePage)}?{nameof(HostCarUpdateViewModel.CarId)}={car.Id}");
        }
    }
}
