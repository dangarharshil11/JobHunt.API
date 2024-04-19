using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;

namespace JobHunt.Services.EmployerAPI
{
    // Mapping Configuration of Models to its Respective Dtos and Vice-Versa
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Employer, EmployerDto>().ReverseMap();
                config.CreateMap<Vacancy, VacancyResponseDto>().ReverseMap();
                config.CreateMap<Vacancy, VacancyRequestDto>().ReverseMap();
                config.CreateMap<UserVacancyRequest, UserVacancyRequestDto>().ReverseMap();
                config.CreateMap<UserVacancyRequest, UserVacancyResponseDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
