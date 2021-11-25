using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Com.Airbnb.Lottie;
using System;
using System.Threading.Tasks;

namespace TidRod.Droid
{
    [Activity(
        Label = "TidRod",
        MainLauncher = true,
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        Icon = "@drawable/splash_logo"
    )]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        public void OnAnimationCancel(Animator animation)
        {
            
        }

        public void OnAnimationEnd(Animator animation)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySplash);

            var animation = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animation.AddAnimatorListener(this);


        }

        
    }
}