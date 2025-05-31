using AkariBeauty.Objects.Enums;

namespace AkariBeauty.Objects.Dtos.Entities
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Salario { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public int EmpresaId { get; set; }
    }

}
