namespace Spacy
{
    public static class Extensions
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