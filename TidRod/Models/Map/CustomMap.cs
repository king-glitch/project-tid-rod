using System;
using System.Collections.Generic;
using System.Text;

namespace TidRod.Components.Map
{
    public class CustomMap : Xamarin.Forms.Maps.Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
