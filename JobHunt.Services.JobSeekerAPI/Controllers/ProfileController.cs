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
    [Route("api/jobSeeker")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        private readonly IUploadRepository _uploadRepository;
        protected ResponseDto _response;

        public ProfileController(IMapper mapper, IProfileRepository profileRepository, IUploadRepository uploadRepository)
        {
            _mapper = mapper;
            _profileRepository = profileRepository;
            _uploadRepository = uploadRepository;
            _response = new();
        }

        // Post Endpoint for retrieving users based on list of userId
        [HttpPost]
        [Route("getUsers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers([FromBody] List<Guid> userList)
        {
            List<User> users = await _profileRepository.GetUsersAsync(userList);
            var response = new List<UserDto>();
            foreach (var user in users)
            {
                response.Add(_mapper.Map<UserDto>(user));
            }
            return Ok(response);
        }

        // Get Endpoint for retrieving particular user profile based on email
        [HttpGet]
        [Route("getByEmail/{email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfileByEmail([FromRoute] string email)
        {
            if (email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                // Checks whether user profile exists or not
                User result = await _profileRepository.GetByEmailAsync(email);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Profile not found";
                }
                else
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        // Get Endpoint for retrieving particular user profile based on userId
        [HttpGet]
        [Route("getByUserId/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfileByUserId([FromRoute] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is Empty";
            }
            else
            {
                // Checks whether user profile exists or not
                User result = await _profileRepository.GetByUserIdAsync(userId);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Profile not found";
                }
                else
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        // Post Endpoint for Creating User profile
        // Only Users with Jobseeker Role are allowed
        [HttpPost]
        [Route("profile")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        public async Task<IActionResult> AddProfile(UserDto user)
        {
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User is Empty";
            }
            else
            {
                User request = _mapper.Map<User>(user);
                var result = await _profileRepository.CreateAsync(request);
                UserDto response = _mapper.Map<UserDto>(result);

                _response.Result = response;
                _response.Message = "User Profile Added Successfully";
            }
            return Ok(_response);
        }

        // Put Endpoint for Updating User profile
        // Only Users with Jobseeker Role are allowed
        [HttpPut]
        [Route("profile")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto user)
        {
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User is Empty";
            }
            else
            {
                User request = _mapper.Map<User>(user);

                var result = await _profileRepository.UpdateAsync(request);
                if (result != null)
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                    _response.Message = "User Profile Updated Successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = " User Profile Update Failed";
                }
            }
            return Ok(_response);
        }

        // Post Endpoint for uploading user Resume
        // Only Users with Jobseeker Role are allowed
        [HttpPost]
        [Route("uploadResume")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile file, [FromForm] string fileName)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var resume = new UploadDto
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName
                };

                resume = await _uploadRepository.UploadResume(file, resume);
                _response.Result = resume.Url;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Resume Upload Model is not Valid";
            }
            return Ok(_response);
        }

        // Post Endpoint for uploading user profile Image
        // Only Users with Jobseeker Role are allowed
        [HttpPost]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [Route("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName)
        {
            ValidateImageUpload(file);

            if (ModelState.IsValid)
            {
                var image = new UploadDto
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName
                };

                image = await _uploadRepository.UploadImage(file, image);
                _response.Result = image.Url;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Image Upload Model is not Valid";
            }
            return Ok(_response);
        }

        // Validate the Image Extension and file size
        private void ValidateImageUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 10MB");
            }
        }

        // Validate the Resume Extension and file size
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".pdf", ".doc" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 5MB");
            }
        }
    }
}
