namespace AkariBeauty.Utils;

public static class FormatString
{
    public static string RemoverNaoNumeros(string str)
    {
        return str.Replace(@"[^0-9]", "");
    }
}
