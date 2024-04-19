using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;

namespace JobHunt.Services.JobSeekerAPI
{
    public class MappingConfig
    {
        // Mapping Configuration of Models to its Respective Dtos and Vice-Versa
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<User, UserDto>().ReverseMap();
                config.CreateMap<Qualification, QualificationResponseDto>().ReverseMap();
                config.CreateMap<Qualification, QualificationRequestDto>().ReverseMap();
                config.CreateMap<UserExperience, UserExperienceRequestDto>().ReverseMap();
                config.CreateMap<UserExperience, UserExperienceResponseDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
