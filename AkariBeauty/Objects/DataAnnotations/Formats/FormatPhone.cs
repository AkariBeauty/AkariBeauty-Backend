using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Formats;

public class FormatPhoneAttribute : BaseAnnotation
{
    public override void Execute()
    {
        string telefone = new string(Value.ToString()?.Where(char.IsDigit).ToArray());
        if (telefone.Length != 11)
            ReturnError("O telefone deve conter pelo menos 11 dígitos.");

        if (telefone.All(c => c == telefone[0]))
            ReturnError("O telefone deve conter dígitos diferentes.");

        SetValue(telefone);
    }
}
