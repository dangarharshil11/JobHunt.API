namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class VacancyResponseDto
    {
        public Guid Id { get; set; }
        public string PublishedBy { get; set; }
        public DateTime PublishedDate { get; set; }
        public int NoOfVacancies { get; set; }
        public string MinimumQualification { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string ExperienceRequired { get; set; }
        public DateTime LastDate { get; set; }
        public double MinimumSalary { get; set; }
        public double MaximumSalary { get; set; }
        public bool Applied { get; set; } = false;
    }
}
