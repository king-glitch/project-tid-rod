using System;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TidRod.Droid.Effects;
using Color = Android.Graphics.Color;

[assembly: ResolutionGroupName("TidRod")]
[assembly: ExportEffect(typeof(RemoveEntryUnderline), nameof(RemoveEntryUnderline))]

namespace TidRod.Droid.Effects
{
    public class RemoveEntryUnderline : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (!(Control is EditText editText))
            {
                throw new NotImplementedException();
            }

            editText.SetBackgroundColor(Color.Transparent);
        }

        protected override void OnDetached()
        {
        }
    }
}