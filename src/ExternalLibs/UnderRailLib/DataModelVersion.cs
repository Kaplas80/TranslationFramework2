namespace UnderRailLib
{
    public static class DataModelVersion
    {
        public static long CurrentDataModelVersion { get; set; }

        public static int MajorVersion => (int) (CurrentDataModelVersion / d);
        public static int MinorVersion => (int) (CurrentDataModelVersion % d);

        private static long d = 10000000000L;
    }
}
