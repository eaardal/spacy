using System;
using System.Windows.Shapes;

namespace Spacy
{
    public class DiskStatus
    {
        private const int Decimals = 0;
        public double AvailableFreeSpaceGb { get; set; }
        public string DriveName { get; set; }
        public string DriveLetter { get; set; }
        public double TotalSpaceGb { get; set; }
        public Rectangle FreeSpaceIndicator { get; set; }
        public Rectangle TotalSpaceIndicator { get; set; }

        public double UsedSpace
        {
            get { return TotalSpaceGb - AvailableFreeSpaceGb; }
        }

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
                return String.Format("Free: {0}/{1} gb ({2}%)", Math.Round(AvailableFreeSpaceGb, Decimals), Math.Round(TotalSpaceGb, Decimals), Math.Round(FreeSpacePercentage, 2));
            }
        }

        public string UsedSpaceText
        {
            get
            {
                return String.Format("Used: {0}/{1} gb ({2}%)", Math.Round(UsedSpace, Decimals), Math.Round(TotalSpaceGb, Decimals), Math.Round(UsedSpacePercentage, 2));
            }
        }

        public string DriveNameText { get { return String.Format("{0} {1}", DriveName, DriveLetter); }}
    }
}