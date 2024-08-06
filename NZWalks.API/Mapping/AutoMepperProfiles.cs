using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mapping
{
    public class AutoMepperProfiles : Profile
    {
        public AutoMepperProfiles()
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO, Region>().ReverseMap();

            //Walks Table
            CreateMap<AddWalksRequestDTO,Walks>().ReverseMap();
            CreateMap<Walks, WalksDTO>().ReverseMap();
            CreateMap<UpdateWalksRequestDTO, Walks>().ReverseMap();

            //Difficulty Table
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
        }
    }
}
