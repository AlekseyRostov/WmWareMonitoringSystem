namespace WMWareApi.Model
{
    public class DatastoreInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Uuid { get; set; }
        public bool? Ssd { get; set; }
        public long FreeSpace { get; set; }
        public long Capacity { get; set; }
        public VirtualDiskInfo VirtualDisk { get; set; }
    }
}