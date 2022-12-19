using AutoMapper;
using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;

namespace Sodimac.Cedis.Core.Mappings
{
    public class AutomapperProfile : Profile
    {
        //TODO: no olvidar instanciar todos los mapper
        public AutomapperProfile()
        {
            CreateMap<Request,RequestDTO>().ReverseMap();
            CreateMap<RequestImp, RequestImpDTO>().ReverseMap();
            CreateMap<Response,ResponseDTO>().ReverseMap();
            CreateMap<Impresora, ImpresoraDTO>().ReverseMap();
            CreateMap<LoteImpresion, LoteImpresionDto>().ReverseMap();
            CreateMap<RqOla, RqOlaDto>().ReverseMap();
            CreateMap<XmlEtq, XmlEtqDto>().ReverseMap();
            CreateMap<OlaPorEstado, OlaPorEstadoDto>().ReverseMap();
        }
    }
}
