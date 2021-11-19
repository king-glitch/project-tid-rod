using System;
using System.Collections.Generic;
using System.Text;

namespace TidRod.Models
{
    public enum CarTransmission
    { Automatic, Manual }

    public class Car
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public int Price { get; set; }
        public int Obometer { get; set; }
        public CarTransmission Gear { get; set; }
        public string PinLocation { get; set; }
        public string UserId { get; set; }
    }
}