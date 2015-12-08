using System.Diagnostics;

namespace Spacy
{
    public static class Bootstrapper
    {
        public static string EventLogIdentifier = "Spacy";

        public static void Configure()
        {
            Config.Load();

            if (!EventLog.SourceExists(EventLogIdentifier))
                EventLog.CreateEventSource(EventLogIdentifier, "Application");
        }
    }
}