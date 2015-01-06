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

        public MainWindow()
        {
            InitializeComponent();

            DiskStatus = new ObservableCollection<DiskStatus>();

            try
            {
                CreateDiskStatus();
                CreateOverallStatus();
                SetHeaderText();
            }
            catch (Exception ex)
            {
                const string cs = "Spacy";
                if (!EventLog.SourceExists(cs))
                    EventLog.CreateEventSource(cs, "Application");

                var msg = String.Format("{0}:\n\n{1}", ex.Message, ex.StackTrace);
                EventLog.WriteEntry(cs, msg, EventLogEntryType.Error);

                MessageBox.Show("An error occurred. This was also written to windows event viewer\n\n" + msg, "Error", MessageBoxButton.OK);
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
                overall.DriveLetter = String.Empty;
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
#if DEBUG
            for (int i = 0; i < 2; i++)
            {
#endif
                foreach (var drive in drives)
                {
                    var diskStatus = new DiskStatus
                    {
                        AvailableFreeSpaceGb = drive.AvailableFreeSpace.ToGb(),
                        DriveName =  String.IsNullOrEmpty(drive.VolumeLabel) ? "[No name]" : drive.VolumeLabel,
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
            var freeSpaceTreshold = int.Parse(ConfigurationManager.AppSettings["FreeSpaceWarningThreshold"]);
            diskStatus.TotalSpaceIndicator.Width = IndicatorWidth*diskStatus.UsedSpacePercentage/100;
            diskStatus.TotalSpaceIndicator.Fill = diskStatus.FreeSpacePercentage < freeSpaceTreshold
                ? Brushes.Red
                : Brushes.LimeGreen;
            diskStatus.FreeSpaceIndicator.Width = IndicatorWidth*diskStatus.FreeSpacePercentage/100;
        }

        private static IEnumerable<DriveInfo> GetDrives()
        {
            var getManually = bool.Parse(ConfigurationManager.AppSettings["GetDrivesManually"]);
            if (getManually)
            {
                var letters = ConfigurationManager.AppSettings["Drives"].ToUpper().Split(',');
                return letters.Select(l => new DriveInfo(l));
            }
            return DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS");
        }
    }
}
