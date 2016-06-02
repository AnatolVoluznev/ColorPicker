using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Content.Res;
using Android.Graphics;
using Android.Content.PM;
using Android;
using ColorPicker.common;

namespace ColorPicker
{
    [Activity(Label = "Colors", Icon = "@drawable/icon",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Portrait)]
    public class ColorDisplay : Activity
    {
        private RgbColor color;
        private AssetManager assets;
        private TextView colorName;
        private SeekBar RSeekBar;
        private SeekBar GSeekBar;
        private SeekBar BSeekBar;
        private TextView colorView;
        private Button backButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ColorDisplay);
            assets = this.Assets;
            colorName = FindViewById<TextView>(Resource.Id.textView);
            backButton = FindViewById<Button>(Resource.Id.backToMain);
            var R = int.Parse(Intent.GetStringExtra("R"));
            var G = int.Parse(Intent.GetStringExtra("G"));
            var B = int.Parse(Intent.GetStringExtra("B"));          
            colorView = FindViewById<TextView>(Resource.Id.colorView);
            RSeekBar = FindViewById<SeekBar>(Resource.Id.RseekBar);
            GSeekBar = FindViewById<SeekBar>(Resource.Id.GseekBar);
            BSeekBar = FindViewById<SeekBar>(Resource.Id.BseekBar);
            RSeekBar.Progress = R;
            GSeekBar.Progress = G;
            BSeekBar.Progress = B;
            showColor(R, G, B);
            RSeekBar.ProgressChanged += delegate {
                showColor(RSeekBar.Progress, GSeekBar.Progress, BSeekBar.Progress);
                };
            GSeekBar.ProgressChanged += delegate {
                showColor(RSeekBar.Progress, GSeekBar.Progress, BSeekBar.Progress);
            };
            BSeekBar.ProgressChanged += delegate {
                showColor(RSeekBar.Progress, GSeekBar.Progress, BSeekBar.Progress);
            };

            backButton.Click += delegate
             {
                 var activity = new Intent(this, typeof(Activity1));
                 StartActivity(activity);
             };

        }

        public void showColor(int R, int G, int B)
        {
            color = new RgbColor(assets);
            color.setRGB(R, G, B);
            colorName.Text = color.getName();          
            Color shownColor = new Color(R, G, B);
            colorView.SetBackgroundColor(shownColor);
        }
        
        }
    

}