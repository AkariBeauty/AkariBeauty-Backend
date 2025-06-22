using System;
using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Formats;

public class FormatCpfAttribute : BaseAnnotation
{
    public override void Execute()
    {
        string cpf = new string(Value.ToString()?.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            ReturnError("O CPF deve conter 11 dígitos.");

        if (cpf.All(c => c == cpf[0]))
            ReturnError("O CPF deve conter dígitos diferentes.");

        int[] digits = new int[11];
        for (int i = 0; i < 11; i++)
        {
            digits[i] = int.Parse(cpf[i].ToString());
        }

        int sum = 0;
        int weight = 10;
        for (int i = 0; i < 9; i++)
        {
            sum += digits[i] * weight;
            weight--;
        }

        int remainder = sum % 11;
        int calculatedDv1 = (remainder < 2) ? 0 : (11 - remainder);

        if (calculatedDv1 != digits[9])
            ReturnError("O CPF deve ser válido.");

        sum = 0;
        weight = 11;
        for (int i = 0; i < 10; i++)
        {
            sum += digits[i] * weight;
            weight--;
        }

        remainder = sum % 11;
        int calculatedDv2 = (remainder < 2) ? 0 : (11 - remainder);

        if (calculatedDv2 != digits[10])
            ReturnError("O CPF deve ser válido.");
            
        SetValue(cpf.ToString());
    }
}
