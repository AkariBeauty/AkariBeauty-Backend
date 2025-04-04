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


        }
    }
}