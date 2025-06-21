using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Formats;

public class FormatRemoveSpaceStartEndAttribute : BaseAnnotation
{
    public override void Execute()
    {
        SetValue(Value?.ToString()?.Trim());
    }
}
