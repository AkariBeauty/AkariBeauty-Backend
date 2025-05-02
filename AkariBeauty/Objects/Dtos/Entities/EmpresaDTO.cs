namespace AkariBeauty.Objects.Dtos.Entities
{
    public class EmpresaDTO
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial {  get; set; }
        public string Uf {  get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero {  get; set; }
        public DateTime HoraInicial { get; set; }
        public DateTime HoraFinal {  get; set; } 
        public bool Adiantamento {  get; set; }

    }

}
