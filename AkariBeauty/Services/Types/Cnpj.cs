namespace AkariBeauty.Services.Types
{
    public class Cnpj
    {
        private readonly string _cnpj;

        public Cnpj(string cnpj)
        {
            // Remove caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray()).Trim();

            if (cnpj.Length != 14)
                throw new ArgumentException("CNPJ deve ter 14 caracteres.");

            // Verifica se todos os caracteres são iguais (CNPJs inválidos comuns)
            if (cnpj.Distinct().Count() == 1)
                throw new ArgumentException("CNPJ inválido.");

            string cnpjSemDigitos = cnpj.Substring(0, 12);
            string digitosOriginais = cnpj.Substring(12, 2);

            string digitosCalculados = CalcularDigitosVerificadores(cnpjSemDigitos);

            if (digitosOriginais != digitosCalculados)
                throw new ArgumentException("CNPJ inválido.");

            _cnpj = cnpj;
        }

        private static string CalcularDigitosVerificadores(string cnpjBase)
        {
            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += (cnpjBase[i] - '0') * multiplicadores1[i];

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            string temp = cnpjBase + primeiroDigito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += (temp[i] - '0') * multiplicadores2[i];

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            return primeiroDigito.ToString() + segundoDigito.ToString();
        }

        public override string ToString()
        {
            return _cnpj;
        }
    }
}