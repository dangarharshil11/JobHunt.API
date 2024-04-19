using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Method for getting Users form JobSekeer Database By Calling JobSeeker Micorservice
        public async Task<List<UserDto>> GetUsers(List<Guid> users)
        {
            var client = _httpClientFactory.CreateClient("Profile");
            var data = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/jobSeeker/getUsers", data);
            var apiContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(apiContent));
        }
    }
}
