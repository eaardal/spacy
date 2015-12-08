using System;
using System.Configuration;
using System.Linq;

namespace Spacy
{
    class Config
    {
        public static bool StartMaximized { get; private set; }
        public static bool OverrideScreenSize { get; private set; }
        public static int ScreenSizeHeight { get; private set; }
        public static int ScreenSizeWidth { get; private set; }
        public static bool DrivesFromAppConfig { get; private set; }
        public static string[] DriveLetters { get; private set; }
        public static int FreeSpaceWarningThresholdInGb { get; private set; }

        public static void Load()
        {
            StartMaximized = GetConfiguration("StartMaximized", false);
            FreeSpaceWarningThresholdInGb = GetConfiguration("FreeSpaceWarningThresholdInGb", 20);
            SetDrivesConfiguration();
            SetScreenSize();
        }

        private static void SetScreenSize()
        {
            OverrideScreenSize = GetConfiguration("OverrideScreenSize", false);
            if (OverrideScreenSize)
            {
                ScreenSizeHeight = GetConfiguration("ScreenHeight", 720);
                ScreenSizeWidth = GetConfiguration("ScreenWidth", 1280);
            }
            else
            {
                ScreenSizeHeight = -1;
                ScreenSizeWidth = -1;
            }
        }

        private static void SetDrivesConfiguration()
        {
            DrivesFromAppConfig = GetConfiguration("DrivesFromAppConfig", false);

            if (DrivesFromAppConfig)
            {
                var driveLetters = GetConfiguration("DriveLetters", string.Empty);
                DriveLetters = !string.IsNullOrEmpty(driveLetters)
                    ? driveLetters.ToUpper().Split(',')
                    : new string[0];
            }
            else
            {
                DriveLetters = new string[0];
            }
        }

        private static T GetConfiguration<T>(string key, T defaultValue)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return (T) Convert.ChangeType(value, typeof (T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
