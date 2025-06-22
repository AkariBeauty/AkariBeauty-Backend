namespace AkariBeauty.Services.Types
{
    public static class CnpjValidade
    {
        public static string Validar(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ não pode ser vazio.");

            // Remove caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                throw new ArgumentException("CNPJ deve ter 14 dígitos.");

            if (cnpj.Distinct().Count() == 1)
                throw new ArgumentException("CNPJ inválido (dígitos repetidos).");

            string baseCnpj = cnpj.Substring(0, 12);
            string digitosOriginais = cnpj.Substring(12, 2);

            string digitosCalculados = CalcularDigitosVerificadores(baseCnpj);

            if (digitosOriginais != digitosCalculados)
                throw new ArgumentException("CNPJ inválido.");

            return cnpj;
        }

        public static string Formatar(string cnpj)
        {
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                throw new ArgumentException("CNPJ inválido para formatação.");

            return $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..]}";
        }

        private static string CalcularDigitosVerificadores(string cnpjBase)
        {
            int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = mult1
                .Select((m, i) => (cnpjBase[i] - '0') * m)
                .Sum();

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            string temp = cnpjBase + digito1;

            soma = mult2
                .Select((m, i) => (temp[i] - '0') * m)
                .Sum();

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return $"{digito1}{digito2}";
        }
    }
}
