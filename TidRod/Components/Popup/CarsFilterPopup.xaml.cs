using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using TidRod.Models;
using TidRod.Services.Interface;
using TidRod.ViewModels;
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

        private void ButtonClicked(object sender, EventArgs e)
        {
            PopAndSearch();
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

            PriceModifierLabel.Text = $"100 THB - {(price >= 10000 ? "10000+" : price.ToString())} THB / day";

            // obomether modifier
            int obemeter = (int)ObometerRangeSlider.Value;

            ObometerModifierLabel.Text = $"0 - {(obemeter >= 500000 ? "500000+" : obemeter.ToString())} / miles";

            // Filter Button
            FilterResultButton.IsEnabled = false;

            List<Car> cars = (List<Car>)(await CarDataStore.GetCarsAsync());

            var filtered =
                cars
                    .FindAll(car =>
                    {
                        bool check = true;

                        if (price > 0)
                        {
                            check = check && price >= 10000 ? car.Price <= 100000 : car.Price <= price;
                        }

                        // if obometer is in filter;

                        if (obemeter > 0)
                        {
                            check = check && obemeter >= 500000 ? car.Obometer <= 2000000 : car.Obometer <= obemeter;
                        }

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
            this.FilterCars();
            PopAndSearch(true);
        }

        private async void PopAndSearch(bool clear = false)
        {
            await Navigation.PopPopupAsync();
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}?{nameof(SearchViewModel.Price)}={(clear ? 0 : (int)PriceRangeSlider.Value)}&{nameof(SearchViewModel.Gear)}={(GearPicker.SelectedIndex == -1 ? "Both" : GearPicker.ItemsSource[GearPicker.SelectedIndex])}&{nameof(SearchViewModel.Obometer)}={(clear ? 0 : (int)ObometerRangeSlider.Value)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
