using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Android.Graphics;
using Android.Content;
using Android.Content.Res;
using ColorPicker.common;


namespace ColorPicker
{
    [Activity (Label = "TextureViewCameraStream", Icon = "@drawable/icon", MainLauncher = true)]
    public class Activity1 : Activity, TextureView.ISurfaceTextureListener
    {
        private AssetManager assets;
        private RgbColor color;
        Android.Hardware.Camera _camera;
        TextureView _textureView;
        
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            assets = this.Assets;
            _textureView = new TextureView (this);
            _textureView.SurfaceTextureListener = this;
            SetContentView (_textureView);
        }
                
        public void OnSurfaceTextureAvailable (SurfaceTexture surface, int w, int h)
        {
            _camera = Android.Hardware.Camera.Open ();

            var previewSize = _camera.GetParameters().PreviewSize;
        
           
            _textureView.LayoutParameters =
                new FrameLayout.LayoutParams(h,
                    w, GravityFlags.Center);
            try {
                _camera.SetPreviewTexture (surface);
                _camera.StartPreview ();
                
            } catch (Java.IO.IOException ex) {
                Console.WriteLine (ex.Message);
            }
            _textureView.Rotation = 90.0f;
            _textureView.Alpha = 0.5f;
        }

        public bool OnSurfaceTextureDestroyed (Android.Graphics.SurfaceTexture surface)
        {
            _camera.StopPreview ();
            _camera.Release ();
            
            return true;
        }

        public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            // camera takes care of this
        }

        public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
        {
            
        }



        public override bool OnTouchEvent(MotionEvent ev)
        {
            int InvalidPointerId = -1;
            int _activePointerId = InvalidPointerId;
            int _lastTouchX;
            int _lastTouchY;

            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            _lastTouchX = (int)ev.GetX();
            _lastTouchY = (int)ev.GetY();
            _activePointerId = ev.GetPointerId(0);
          
            getImageColor(_lastTouchX, _lastTouchY);
            return false;
        }
        private void getImageColor(int x, int y)
        {
            int[] pixels = new int[100];
            Bitmap bitmap = _textureView.Bitmap;
            int a = bitmap.Height;
            int b = bitmap.Width;       
            int k = 0;
            for (int i = x - 5; i < x + 5 && i<bitmap.Height; i++)
            {
                for (int j = y - 5; j < y + 5 && j<bitmap.Width; j++)
                {
                    pixels[k] = bitmap.GetPixel(j, i);
                    k++;
                }
            }
            setRgbColor(pixels);
        }
        private void showColor()
        {
            var activity = new Intent(this, typeof(ColorDisplay));
            if (color == null)
            {
                activity.PutExtra("R", 255.ToString());
                activity.PutExtra("G", 255.ToString());
                activity.PutExtra("B", 255.ToString());
                activity.PutExtra("name", "белый");
                StartActivity(activity);
            }
            else
            {
                activity.PutExtra("R", color.getR().ToString());
                activity.PutExtra("G", color.getG().ToString());
                activity.PutExtra("B", color.getB().ToString());
                activity.PutExtra("name", color.getName());
            }
            StartActivity(activity);
        }
        public void setRgbColor(int[] pixels)
        {
            int pixel = 0;
            int R = 0;
            int G = 0;
            int B = 0;
            int noizePixel = pixels[0];
            color = new RgbColor(assets);

            for (int i = 1; i < pixels.Length; i++)
            {
                pixel = pixels[i];
                pixel = checkNoizePixel(noizePixel, pixel);
                R = Color.GetRedComponent(pixel);
                G = Color.GetGreenComponent(pixel);
                B = Color.GetBlueComponent(pixel);
            }


            color.setRGB(R, G, B);
            showColor();
        }
        public int checkNoizePixel(int noizePixel, int pixel)
        {
            int noizePixelClarity = getClarity(noizePixel);
            int pixelClarity = getClarity(pixel);
            if (noizePixelClarity > pixelClarity)
            {
                return noizePixel;
            }
            else return pixel;
        }

        public int getClarity(int pixel)
        {
            return (Math.Abs(Color.GetRedComponent(pixel) - Color.GetGreenComponent(pixel))) +
                          (Math.Abs(Color.GetRedComponent(pixel) - Color.GetBlueComponent(pixel))) +
                          (Math.Abs(Color.GetGreenComponent(pixel) - Color.GetBlueComponent(pixel)));

        }


    }
}


