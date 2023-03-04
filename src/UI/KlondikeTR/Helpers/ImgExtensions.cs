namespace KlondikeTR.Helpers
{
    public static class ImgExtensions
    {
        public static string ToImgSrc(this byte[]? data)
        {
            var stringData = Convert.ToBase64String(data ?? Array.Empty<byte>());

            return $"data:image;base64,{stringData}";
        }

        public static string GetSrc(byte[]? data)
        {
            var stringData = Convert.ToBase64String(data ?? Array.Empty<byte>());

            return $"data:image;base64,{stringData}";
        }
    }
}
