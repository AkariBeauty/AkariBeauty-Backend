using AkariBeauty.Objects.Dtos.Entities;
using AkariBeauty.Objects.Models;
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

            CreateMap<FuncionarioDTO, Funcionario>().ReverseMap();
            CreateMap<Funcionario, FuncionarioDTO>();
            
            CreateMap<EmpresaDTO, Empresa>().ReverseMap();
            CreateMap<Empresa, EmpresaDTO>();
            
            CreateMap<ClienteDTO, Cliente>().ReverseMap();
            CreateMap<Cliente, ClienteDTO>();
           
            CreateMap<AgendamentoDTO, Agendamento>().ReverseMap();
            CreateMap<Agendamento, AgendamentoDTO>();


        }
    }
}