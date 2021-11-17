using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace TidRod.Models
{
    public class PinInfo
    {
        public Position Position { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
