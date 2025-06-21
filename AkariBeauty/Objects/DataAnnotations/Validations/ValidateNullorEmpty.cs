using AkariBeauty.Objects.Dtos.DataAnnotations.Base;

namespace AkariBeauty.Objects.DataAnnotations.Validations
{
    public class ValidateNullOrEmptyAttribute : BaseAnnotation
    {
        public override void Execute()
        {

            string valor = Value?.ToString()?.Replace(" ", "");

            if (Value is null || valor == "")
                ReturnError("O campo nao pode ser nulo ou vazio");
        }
    }
}