using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spacy
{
    public partial class MainWindow
    {
        private const int IndicatorWidth = 300;
        public ObservableCollection<DiskStatus> DiskStatus { get; set; }
        public string HeaderText { get; set; }
        public DiskStatus OverallStatus { get; set; }
        public WindowSize WindowSize { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Bootstrapper.Configure();

            DiskStatus = new ObservableCollection<DiskStatus>();

            try
            {
                CreateDiskStatus();
                CreateOverallStatus();
                SetHeaderText();
                WindowSize = WindowSize.GetWindowSize(DiskStatus.Count);
            }
            catch (Exception ex)
            {
                var msg = string.Format("{0}:\n\n{1}", ex.Message, ex.StackTrace);
                EventLog.WriteEntry(Bootstrapper.EventLogIdentifier, msg, EventLogEntryType.Error);
                MessageBox.Show("An error occurred. This was also written to windows event viewer\n\n" + msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DataContext = this;
        }

        private void CreateOverallStatus()
        {
            var overall = new DiskStatus();

            foreach (var status in DiskStatus)
            {
                overall.AvailableFreeSpaceGb += status.AvailableFreeSpaceGb;
                overall.TotalSpaceGb += status.TotalSpaceGb;
                overall.DriveName = "Overall status";
                overall.DriveLetter = string.Empty;
                overall.FreeSpaceIndicator = new Rectangle {Fill = Brushes.LightGray};
                overall.TotalSpaceIndicator = new Rectangle();
            }

            SetSpaceIndicators(overall);

            OverallStatus = overall;
        }

        private void SetHeaderText()
        {
            HeaderText = "Disk status for " + Environment.MachineName.ToLower();
        }

        private void CreateDiskStatus()
        {
            var drives = GetDrives().ToList();

// If in debug mode, create more drives to test with
#if DEBUG
            drives = drives.Take(1).ToList();
            for (var i = 0; i < 21; i++)
            {
#endif
                foreach (var drive in drives)
                {
                    var diskStatus = new DiskStatus
                    {
                        AvailableFreeSpaceGb = drive.AvailableFreeSpace.ToGb(),
                        DriveName =  string.IsNullOrEmpty(drive.VolumeLabel) ? "[No name]" : drive.VolumeLabel,
                        DriveLetter = drive.Name.Substring(0, 2),
                        TotalSpaceGb = drive.TotalSize.ToGb(),
                        FreeSpaceIndicator = new Rectangle { Fill = Brushes.LightGray },
                        TotalSpaceIndicator = new Rectangle()
                    };

                    SetSpaceIndicators(diskStatus);

                    DiskStatus.Add(diskStatus);
                }    
#if DEBUG
            }
#endif   
        }

        private static void SetSpaceIndicators(DiskStatus diskStatus)
        {
            var freeSpaceTreshold = Config.FreeSpaceWarningThresholdInGb;
            diskStatus.TotalSpaceIndicator.Width = IndicatorWidth*diskStatus.UsedSpacePercentage/100;
            diskStatus.TotalSpaceIndicator.Fill = diskStatus.FreeSpacePercentage < freeSpaceTreshold
                ? Brushes.Red
                : Brushes.LimeGreen;
            diskStatus.FreeSpaceIndicator.Width = IndicatorWidth*diskStatus.FreeSpacePercentage/100;
        }

        private static IEnumerable<DriveInfo> GetDrives()
        {
            if (Config.DrivesFromAppConfig)
            {
                return Config.DriveLetters.Select(name => new DriveInfo(name));
            }
            return DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS");
        }
    }
}
