﻿namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class UserVacancyResponseDto
    {
        public Guid Id { get; set; }
        public Guid VacancyId { get; set; }
        public Vacancy? Vacancy { get; set; }
        public UserDto? User { get; set; }
        public Guid UserId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public int? TotalRecords { get; set; }
    }
}
