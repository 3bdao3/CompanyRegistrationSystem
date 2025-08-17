using AutoMapper;
using CompanyRegistrationSystem.Application.Dtos;
using CompanyRegistrationSystem.Domain.Entities;

namespace CompanyRegistrationSystem.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanySignUpDto, Company>();

            
            CreateMap<Company, CompanySignUpDto>();

            CreateMap<LoginDto, Company>();

        }
    }
}
