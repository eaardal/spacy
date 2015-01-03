using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spacy
{
    public partial class MainWindow
    {
        private const int IndicatorWidth = 300;
        public ObservableCollection<DiskStatus> DiskStatus { get; set; }
        public Rectangle FreeSpaceIndicator { get; set; }
        public Rectangle TotalSpaceIndicator { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DiskStatus = new ObservableCollection<DiskStatus>();
            
            var drives = DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS");

            foreach (var drive in drives)
            {
                var diskStatus = new DiskStatus
                {
                    AvailableFreeSpaceGb = drive.AvailableFreeSpace.ToGb(),
                    DriveName = drive.VolumeLabel,
                    DriveLetter = drive.Name,
                    TotalSpaceGb = drive.TotalSize.ToGb(),
                };

                diskStatus.FreeSpaceIndicator = new Rectangle();
                diskStatus.TotalSpaceIndicator = new Rectangle();

                diskStatus.FreeSpaceIndicator.Fill = Brushes.LightGray;
                diskStatus.TotalSpaceIndicator.Width =  IndicatorWidth * diskStatus.UsedSpacePercentage / 100;

                if (diskStatus.UsedSpacePercentage > 80)
                {
                    diskStatus.TotalSpaceIndicator.Fill = Brushes.Red;
                }
                else
                {
                    diskStatus.TotalSpaceIndicator.Fill = Brushes.LimeGreen;
                }
                diskStatus.FreeSpaceIndicator.Width = IndicatorWidth * diskStatus.FreeSpacePercentage / 100;

                Debug.WriteLine("FS: " + diskStatus.FreeSpacePercentage + "%");
                Debug.WriteLine("FSI W: " + diskStatus.FreeSpaceIndicator.Width);
                Debug.WriteLine("TSI W: " + diskStatus.TotalSpaceIndicator.Width);
                Debug.WriteLine("Tot W: " + (diskStatus.FreeSpaceIndicator.Width + diskStatus.TotalSpaceIndicator.Width));
                DiskStatus.Add(diskStatus);
            }

            DataContext = this;
        }
    }

    public class DiskStatus
    {
        public double AvailableFreeSpaceGb { get; set; }
        public string DriveName { get; set; }
        public string DriveLetter { get; set; }
        public double TotalSpaceGb { get; set; }
        public Rectangle FreeSpaceIndicator { get; set; }
        public Rectangle TotalSpaceIndicator { get; set; }
        public double FreeSpacePercentage
        {
            get
            {
                return AvailableFreeSpaceGb*100/TotalSpaceGb;
            }
        }

        public double UsedSpacePercentage
        {
            get { return 100 - FreeSpacePercentage; }
        }

        public string AvailableFreeSpaceText
        {
            get
            {
                return String.Format("{0}gb / {1}gb ({2}%) free - {3}% used", Math.Round(AvailableFreeSpaceGb, 2), Math.Round(TotalSpaceGb, 2), Math.Round(FreeSpacePercentage, 2), Math.Round(UsedSpacePercentage, 2));
            }
        }

        public string DriveNameText { get { return String.Format("{0} {1}", DriveName, DriveLetter); }}
    }

    public static class Ext
    {
        public static double ToMb(this long bytes)
        {
            return bytes / 1024 / 1024;
        }

        public static double ToGb(this long bytes)
        {
            return bytes.ToMb() / 1024;
        }

        public static double ToTb(this long bytes)
        {
            return bytes.ToGb() / 1024;
        }
    }
}
