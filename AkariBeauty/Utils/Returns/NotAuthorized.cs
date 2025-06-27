using System;

namespace AkariBeauty.Utils.Returns;

public class NotAuthorized : Exception
{
    public NotAuthorized(string message) : base(message) { }
}
