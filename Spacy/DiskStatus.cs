using System;
using System.Windows.Shapes;

namespace Spacy
{
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
}