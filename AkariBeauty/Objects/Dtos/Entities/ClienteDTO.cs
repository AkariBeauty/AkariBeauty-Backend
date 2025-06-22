namespace AkariBeauty.Objects.Dtos.Entities
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
    }
}
