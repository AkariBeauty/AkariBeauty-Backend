using Microsoft.AspNetCore.Mvc;

namespace AkariBeauty.Utils;

public class GetToken : Controller
{
    public string? Token { get; set; }

    public GetToken()
    {
        Token = Request.Headers["Authorization"];
    }
}
