using System;
using System.Diagnostics;
using System.Windows;

namespace Spacy
{
    public class WindowSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public WindowState StartupState { get; set; }

        public static WindowSize GetWindowSize(int nrOfDrives)
        {
            var windowSize = new WindowSize
            {
                StartupState = Config.StartMaximized ? WindowState.Maximized : WindowState.Normal
            };
            
            if (Config.OverrideScreenSize)
            {
                windowSize.Width = Config.ScreenSizeWidth;
                windowSize.Height = Config.ScreenSizeHeight;
                return windowSize;
            }
            
            if (nrOfDrives > 1 && nrOfDrives <= 6)
            {
                Debug.WriteLine(nrOfDrives + " drives = 2x3 grid");
                return windowSize.ForTilesHorizontally(2).ForTilesVertically(3);
            }
            if (nrOfDrives > 6 && nrOfDrives <= 12)
            {
                Debug.WriteLine(nrOfDrives + " drives = 4x3 grid");
                return windowSize.ForTilesHorizontally(4).ForTilesVertically(3);
            }
            if (nrOfDrives > 12 && nrOfDrives <= 18)
            {
                Debug.WriteLine(nrOfDrives + " drives = 4x5 grid");
                return windowSize.ForTilesHorizontally(4).ForTilesVertically(5);
            }

            return FullHD(windowSize);
        }

        private static WindowSize FullHD(WindowSize windowSize)
        {
            windowSize.Width = 1920;
            windowSize.Height = 720;
            return windowSize;
        }
    }
}
