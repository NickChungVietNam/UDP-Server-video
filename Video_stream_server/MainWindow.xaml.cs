using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




//webcam 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections.ObjectModel;
using Microsoft.Graph;
using System.ComponentModel;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections.ObjectModel;
using Microsoft.Graph;
using System.ComponentModel;


using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

//TCP/UDP
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing.Drawing2D;
using SharpDX.DirectWrite;


namespace Video_stream_server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {







        const int max_size_data_video_tr = 10000000;

        class buffer_st_sender
        {

            public byte[] data_video_tr0;
            public int Size_data_video_tr0 = 0;
            public byte[] data_video_tr1;
            public int Size_data_video_tr1 = 0;
            public buffer_st_sender()
            {
                data_video_tr0 = new byte[max_size_data_video_tr];
                Size_data_video_tr0 = 0;
                data_video_tr1 = new byte[max_size_data_video_tr];
                Size_data_video_tr1 = 0;
            }

            public void Add_byte_to_buffer_st_sender(byte[] input)
            {

                if (input.Length > max_size_data_video_tr)
                {
                    return;
                }
                //

                if (Size_data_video_tr0 == 0)
                {
                    // input .CopyTo(data_video_tr0, 0);

                    Array.Copy(input, data_video_tr0, input.Length);
                    Size_data_video_tr0 = input.Length;
                    //  Console.WriteLine("Send 3454  = " + input.Length);
                }
                else if (Size_data_video_tr1 == 0)
                {
                    //  input .CopyTo(data_video_tr1, 0);

                    Array.Copy(input, data_video_tr1, input.Length);
                    Size_data_video_tr1 = input.Length;
                    //  Console.WriteLine("Send 789 = " + input.Length);
                }
            }










            public byte[] Header_gg =
                           {
            0,1,2,3,4,
            5,6,7,8,9,
            10,11,12,13,14,
            15,16,17,18,19,
            0,1,2,3,4,
            5,6,7,8,9,
            10,11,12,13,14,
            15,16,17,18,19
                        };

            public byte[] end_gg =
                           {
                            19,18,17,16,15,
                            14,13,12,11,10,
                             9,8,7,6,5,
                             4,3,2,1,0 ,
                            19,18,17,16,15,
                            14,13,12,11,10,
                             9,8,7,6,5,
                             4,3,2,1,0
                       };

            public int size_add_detect_header = 0;
            public int size_add_detect_end = 0;
            public bool Found_header = false;

            public byte[] buffer_STREAM_in = new byte[max_size_data_video_tr];
            public int Countsd_buffer_STREAM_in = 0;


            public int Parse_stream_video_in(int read_in)
            {
                if (read_in < 0)
                {
                    return 0;
                }

                if (!Found_header)
                {
                    if (Header_gg[size_add_detect_header] == read_in)
                    {



                        size_add_detect_header++;
                        if (size_add_detect_header >= Header_gg.Length)
                        {
                            Found_header = true;
                            size_add_detect_header = 0;
                            Console.WriteLine("Server rec = " + Countsd_buffer_STREAM_in);
                        }
                    }
                    else
                    {
                        size_add_detect_header = 0;
                    }
                }
                else
                {
                    buffer_STREAM_in[Countsd_buffer_STREAM_in] = (byte)read_in;
                    Countsd_buffer_STREAM_in++;


                    if (end_gg[size_add_detect_end] == read_in)
                    {
                        size_add_detect_end++;
                    }
                    else
                    {
                        size_add_detect_end = 0;
                    }
                    if (
                        //(Countsd_buffer_STREAM_in >= (57653 + size_add_detect_end)) ||
                        (size_add_detect_end >= end_gg.Length) ||
                         (Countsd_buffer_STREAM_in >= max_size_data_video_tr))
                    {
                        //  Countsd_buffer_STREAM_in >= 57654 + size_add_detect_end

                        //  Console.WriteLine("Server rec = " + (Countsd_buffer_STREAM_in- size_add_detect_end));


                        int size_s = Countsd_buffer_STREAM_in - size_add_detect_end;
                        Countsd_buffer_STREAM_in = 0;
                        Found_header = false;
                        size_add_detect_header = 0;
                        size_add_detect_end = 0;

                        if (size_s == 57654)
                        {

                            return size_s;

                        }


                    }


                }
                return 0;
            }
        }



        buffer_st_sender tcp_bs = new buffer_st_sender();
        buffer_st_sender udp_bs = new buffer_st_sender();


        public Bitmap Resize_Bitmap(Bitmap original_bitmap, int Rizse)
        {

            Bitmap bitmap_resized = new Bitmap(original_bitmap, new System.Drawing.Size(
                                               original_bitmap.Width / Rizse,
                                               original_bitmap.Height / Rizse));

            return bitmap_resized;
        }

        public byte[] BitmapToByteArray(Bitmap imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }
        public Bitmap byteArrayToBitmap(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Bitmap returnImage = new Bitmap(ms);



            return returnImage;
        }

        public Bitmap DataToBitmap(byte[] data, int W, int H)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            // https://www.tek-tips.com/viewthread.cfm?qid=1264492
            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);


            //Unlock the pixels
            bmp.UnlockBits(bmpData);


            //Return the bitmap 
            return bmp;
        }


        const int bufSize = 800 * 1024;
        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }






        void Serverthread_UDP()
        {
            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            State state = new State();
            EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
            AsyncCallback recv = null;
            string address = "127.0.0.1";
            int port = 27000;
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));

            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {

                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);

                Console.WriteLine("Server Receive  = " + bytes);
                for (long i = 0; i < bytes; i++)
                {



                    if (udp_bs.Parse_stream_video_in(so.buffer[i]) == 57654)
                    {

                        try
                        {


                            byte[] arrray_bitmap = new byte[57654];

                            Array.Copy(udp_bs.buffer_STREAM_in, arrray_bitmap, 57654);



                            Bitmap arar = byteArrayToBitmap(arrray_bitmap);



                            BitmapImage bi = arar.ToBitmapImage(0);

                            bi.Freeze(); // avoid cross thread operations and prevents leaks
                            Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer1.Source = bi; }));







                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }



                }

                // Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));

                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
            }, state);

        }



        private void Start_Serverthread_UDP()
        {
            Thread t = new Thread(Serverthread_UDP);
            t.Start();
            // _processingImage = true;
        }



        public MainWindow()
        {
            InitializeComponent();



            Start_Serverthread_UDP();

        }
    }
}
