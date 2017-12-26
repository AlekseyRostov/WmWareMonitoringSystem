namespace OwinWebApiClient.Helper
{
    public static class FormatHelper
    {
        public static string FormatBytes(long? bytes)
        {
            long source = bytes ?? 0;
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = source;
            for (i = 0; i < suffix.Length && source >= 1024; i++, source /= 1024)
            {
                dblSByte = source / 1024.0;
            }

            return $"{dblSByte:0.##} {suffix[i]}";
        }
    }
}