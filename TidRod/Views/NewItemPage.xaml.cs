using System;
using System.Collections.Generic;
using System.ComponentModel;
using TidRod.Models;
using TidRod.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TidRod.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
