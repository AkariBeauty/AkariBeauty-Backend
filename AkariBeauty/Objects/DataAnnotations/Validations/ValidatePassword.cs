using System;
using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Validations;

public class ValidatePasswordAttribute : BaseAnnotation
{
    public override void Execute()
    {
        string password = Value?.ToString();
        if (password.Length < 8)
            ReturnError("A senha deve conter pelo menos 8 caracteres.");

        if (!password.Any(char.IsUpper))
            ReturnError("A senha deve conter pelo menos uma letra maiúscula.");

        if (!password.Any(char.IsLower))
            ReturnError("A senha deve conter pelo menos uma letra minúscula.");

        if (!password.Any(char.IsDigit))
            ReturnError("A senha deve conter pelo menos um número.");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            ReturnError("A senha deve conter pelo menos um caractere especial.");
    }
}
