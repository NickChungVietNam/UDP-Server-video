using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Media.Imaging;


namespace Video_stream_server
{
    static class BitmapHelpers
    {
        public static BitmapImage ToBitmapImage(this Bitmap original_bitmap, int Rizse_for_faster_render_GPU = 0)
        {




            BitmapImage bi = new BitmapImage();

            //Bitmap original = (Bitmap)Image.FromFile("DSC_0002.jpg");
            // Bitmap resized = new Bitmap(original, new Size(original.Width / 4, original.Height / 4));
            // resized.Save("DSC_0002_thumb.jpg");



            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            if (Rizse_for_faster_render_GPU > 0)
            {   //ảnh hưởng đến tốc độ vẽ lên GPU 

                Bitmap bitmap_resized = new Bitmap(original_bitmap, new System.Drawing.Size(
                                                   original_bitmap.Width / Rizse_for_faster_render_GPU,
                                                   original_bitmap.Height / Rizse_for_faster_render_GPU
                                                   ));
                bitmap_resized.Save(ms, ImageFormat.Bmp);
                // https://stackoverflow.com/questions/10442269/scaling-a-system-drawing-bitmap-to-a-given-size-while-maintaining-aspect-ratio
            }
            else
            {
                original_bitmap.Save(ms, ImageFormat.Bmp);

            }

            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;


            /*
            try
            {
                //   System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)value;
                // https://stackoverflow.com/questions/41484041/c-sharp-wpf-aforge-how-to-make-video-smooth
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                if (original_bitmap.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.MemoryBmp.Guid)
                    original_bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                else
                    original_bitmap.Save(ms, original_bitmap.RawFormat);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                return bi;
            }
            catch
            {
                return null;
            }*/

        }



    }
}
