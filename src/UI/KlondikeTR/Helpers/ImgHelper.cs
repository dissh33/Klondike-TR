namespace KlondikeTR.Helpers
{
    public static class ImgHelper
    {
        public static string GetSrc(byte[]? data)
        {
            var stringData = Convert.ToBase64String(data);

            return $"data:image;base64,{stringData}";
        }
    }
}
