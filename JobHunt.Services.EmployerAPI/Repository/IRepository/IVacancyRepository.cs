﻿using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>?> GetAllAsync();
        Task<Vacancy?> GetByIdAsync(Guid id);
        Task<List<Vacancy>?> GetByNameAsync(string name);
        Task<Vacancy> CreateAsync(Vacancy vacancy);
        Task<Vacancy?> UpdateAsync(Vacancy vacancy);
    }
}
