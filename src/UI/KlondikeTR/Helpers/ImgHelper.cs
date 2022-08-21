namespace KlondikeTR.Helpers
{
    public static class ImgHelper
    {
        public static string GetSrc(byte[]? data)
        {
            var stringData = Convert.ToBase64String(data ?? Array.Empty<byte>());

            return $"data:image;base64,{stringData}";
        }

        public static string ToImageSrc(this byte[]? data)
        {
            var stringData = Convert.ToBase64String(data ?? Array.Empty<byte>());

            return $"data:image;base64,{stringData}";
        }
    }
}
