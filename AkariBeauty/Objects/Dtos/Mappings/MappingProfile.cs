using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
using AkariBeauty.Objects.Relationship;
using AutoMapper;

namespace AkariBeauty.Objectcs.Dtos.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Colocar as classes para funfar
            CreateMap<ServicoDTO, Servico>().ReverseMap();
            CreateMap<Servico, ServicoDTO>();

            CreateMap<UsuarioDTO, Usuario>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<EmpresaDTO, Empresa>().ReverseMap();
            CreateMap<Empresa, EmpresaDTO>();

            CreateMap<ClienteDTO, Cliente>().ReverseMap();
            CreateMap<Cliente, ClienteDTO>();

            CreateMap<AgendamentoDTO, Agendamento>().ReverseMap();
            CreateMap<Agendamento, AgendamentoDTO>();

            CreateMap<CategoriaServicoDTO, CategoriaServico>().ReverseMap();
            CreateMap<CategoriaServico, CategoriaServicoDTO>();

            CreateMap<ProfissionalDTO, Profissional>().ReverseMap();
            CreateMap<Profissional, ProfissionalDTO>();

            CreateMap<ProfissionalServicoDTO, ProfissionalServico>().ReverseMap();
            CreateMap<ProfissionalServico, ProfissionalServicoDTO>();

        }
    }
}