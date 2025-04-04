using System.ComponentModel.DataAnnotations.Schema;



namespace AkariBeauty.Objects.Models
{
    [Table("servico")]
    public class Servico
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("servicoprestado")]
        public string ServicoPrestado { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("valorbase")]
        public float ValorBase { get; set; }

        public Servico() { }

        public Servico(int id, string servicoPrestado, string descricao, float valorBase)
        {
            Id = id;
            ServicoPrestado = servicoPrestado;
            Descricao = descricao;
            ValorBase = valorBase;
        }
    }
}
