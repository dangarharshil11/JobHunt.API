using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using JobHunt.Services.JobSeekerAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/experience")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExperienceRepository _experienceRepository;
        protected ResponseDto _response;

        public ExperienceController(IMapper mapper, IExperienceRepository experienceRepository)
        {
            _mapper = mapper;
            _experienceRepository = experienceRepository;
            _response = new();
        }

        // Get Endpoint for retrieving all Experiences of a user
        [HttpGet]
        [Route("getAllExperiencesByUserId/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllExperiencesByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is Empty";
            }
            else
            {
                List<UserExperience> result = await _experienceRepository.GetAllByUserIdAsync(userId);
                List<UserExperienceResponseDto> response = [];

                foreach (var item in result)
                {
                    response.Add(_mapper.Map<UserExperienceResponseDto>(item));
                }
                _response.Result = response;
            }
            return Ok(_response);
        }

        // Get Endpoint for retrieving particular Experience of a user
        [HttpGet]
        [Route("getExperienceById/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExperienceById(Guid id)
        {
            if (id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                var result = await _experienceRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No Experiences Found";
                }
                else
                {
                    var response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        // Post Endpoint for creating user Experience
        // Only Users with Jobseeker Role are allowed
        [HttpPost]
        [Route("addExperience")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddExperience([FromBody] UserExperienceRequestDto request)
        {
            if (request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                UserExperience experience = _mapper.Map<UserExperience>(request);
                var result = await _experienceRepository.CreateAsync(experience);

                var response = _mapper.Map<UserExperienceResponseDto>(result);
                _response.Result = response;
                _response.Message = "Experience Added Successfully";
            }
            return Ok(_response);
        }

        // Put Endpoint for updating user Experience
        // Only Users with Jobseeker Role are allowed
        [HttpPut]
        [Route("experience/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateExperience([FromBody] UserExperienceRequestDto request, [FromRoute] Guid id)
        {
            if (id == Guid.Empty || request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Id or Request is Empty";
            }
            else
            {
                UserExperience experience = _mapper.Map<UserExperience>(request);
                experience.Id = id;

                // Checks whether user experience exists or not
                var result = await _experienceRepository.UpdateAsync(experience);
                if (result != null)
                {
                    UserExperienceResponseDto response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Experience Updated Successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                }
            }
            return Ok(_response);
        }


        // Delete Endpoint for deleting user Experience
        // Only Users with Jobseeker Role are allowed
        [HttpDelete]
        [Route("experience/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteExperience([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                // Checks whether user experience exists or not
                UserExperience experience = await _experienceRepository.GetByIdAsync(id);
                if (experience == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Experience Not Found";
                }
                else
                {
                    var result = await _experienceRepository.DeleteAsync(experience);
                    UserExperienceResponseDto response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Experience Deleted Successfully";
                }
            }
            return Ok(_response);
        }
    }
}
