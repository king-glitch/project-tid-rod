using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TidRod.Models
{
    public class FileImage
    {
        public string FileName { get; set; }
        public string FileURL { get; set; }
        public ImageSource Image { get; set; }
    }
}
