namespace AkariBeauty.Authentication
{
    public class JwtSettings
    {

        public string Issuer { get; } = "AkariBeautyAPI";
        public string Audience { get; } = "AkariBeautyERPCRM";
        public string Key { get; } = "AkariBeautyAPI_Barrament_Users_Authentication";

    }
}
