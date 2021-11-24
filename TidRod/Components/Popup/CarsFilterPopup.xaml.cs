using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using TidRod.Models;
using TidRod.Services.Interface;
using TidRod.ViewModels;
using TidRod.ViewModels.Profile;
using TidRod.Views.Profile;
using TidRod.Views.Search;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Components.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarsFilterPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private ICarDataStore<Car> CarDataStore =>
            DependencyService.Get<ICarDataStore<Car>>();

        public CarsFilterPopup()
        {
            InitializeComponent();
            InitializePicker();

            BindingContext = this;
        }

        private void InitializePicker()
        {
            GearPicker.ItemsSource =
                new List<string> { "Automatic", "Manual", "Both" };
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}?{nameof(SearchViewModel.Price)}={(int)PriceRangeSlider.Value}&{nameof(SearchViewModel.Gear)}={GearPicker.ItemsSource[GearPicker.SelectedIndex]}&{nameof(SearchViewModel.Obometer)}={(int)ObometerRangeSlider.Value}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void FilterChangedValue(object sender, ValueChangedEventArgs e)
        {
            this.FilterCars();
        }

        private void GearPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            this.FilterCars();
        }

        private async void FilterCars()
        {
            // Price modifier
            int price = (int)PriceRangeSlider.Value;

            PriceModifierLabel.Text = $"100 THB - {price} THB / day";

            // obomether modifier
            int obemeter = (int)ObometerRangeSlider.Value;

            ObometerModifierLabel.Text = $"100 - {obemeter} / miles";

            // Filter Button
            FilterResultButton.IsEnabled = false;

            List<Car> cars = (List<Car>)(await CarDataStore.GetCarsAsync());

            var filtered =
                cars
                    .FindAll(car =>
                    {
                        bool check =
                            car.Price <= price && car.Obometer <= obemeter;
                        if (GearPicker.SelectedIndex >= 0)
                        {
                            string gear =
                                GearPicker
                                    .ItemsSource[GearPicker.SelectedIndex]
                                    .ToString();
                            if (gear == "Both")
                            {
                                return check;
                            }
                            return check &&
                            car.Gear ==
                            (
                            gear == "Automatic"
                                ? CarTransmission.Automatic
                                : CarTransmission.Manual
                            );
                        }

                        return check;
                    });

            FilterResultButton.Text = $"View {filtered.Count}+ results";

            FilterResultButton.IsEnabled = filtered.Count > 0;
        }

        private void FilterResultButtonClicked(object sender, EventArgs e)
        {
            PriceRangeSlider.Value = 0;
            ObometerRangeSlider.Value = 0;
            GearPicker.SelectedIndex = -1;
            FilterResultButton.Text = $"View 0 results";
            this.FilterCars();
        }
    }
}
