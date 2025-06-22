using System;
using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Validations;

public class ValidateQtdCaractersAttribute : BaseAnnotation
{

    public ValidateQtdCaractersAttribute(params object[]? parameters) : base(parameters)
    {
        if (parameters is null)
            throw new ArgumentNullException("Essa funcão precisa de parametros");

    }

    public override void Execute()
    {
        var qtdValor = Value?.ToString()?.Length;
        if (Parameters != null)
        {
            foreach (var item in Parameters)
            {
                if (qtdValor == (int)item)
                    return;
            }
        }

        ReturnError();

    }
}
