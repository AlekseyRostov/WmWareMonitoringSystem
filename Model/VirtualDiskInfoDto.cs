namespace Model
{
    public class VirtualDiskInfoDto
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public int Partition { get; set; }

        public long? FreeSpace { get; set; }

        public long? Capacity { get; set; }
    }
}