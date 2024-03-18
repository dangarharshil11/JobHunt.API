﻿using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserVacancyRequest> CreateAsync(UserVacancyRequest request)
        {
            await _db.UserVacancyRequests.AddAsync(request);
            await _db.SaveChangesAsync();

            return request;
        }

        public async Task<List<UserVacancyRequest>> GetAllByUserIdAsync(Guid userId)
        {
            var result = await _db.UserVacancyRequests.Where(request => request.UserId == userId).Include(u => u.Vacancy).ToListAsync();
            return result;
        }

        public async Task<List<UserVacancyRequest>> GetAllByVacancyIdAsync(Guid vacancyId)
        {
            var result = await _db.UserVacancyRequests.Where(request => request.VacancyId == vacancyId).Include(u => u.Vacancy).ToListAsync();
            return result;
        }

        public async Task<UserVacancyRequest?> GetDetailAsync(Guid userId, Guid vacancyId)
        {
            return await _db.UserVacancyRequests.FirstOrDefaultAsync(u => u.VacancyId == vacancyId && u.UserId == userId);
        }

        public async Task<UserVacancyRequest?> GetDetailByIdAsync(Guid id)
        {
            return await _db.UserVacancyRequests.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<UserVacancyRequest>> GetAllVacnacyByPageAsync(SP_VacancyRequestDto request)
        {
            var result = _db.UserVacancyRequests.FromSql($"SP_JobApplications @vacancyId = {request.VacancyId}, @page = {request.PageNumber}, @recordsperpage = {request.PageSize}").ToList();
            List<UserVacancyRequest> response = result;
            foreach (var item in response)
            {
                item.Vacancy = await _db.VacancyDetails.FirstOrDefaultAsync(u => u.Id == item.VacancyId);
            }

            return response;
        }

        public async Task<UserVacancyRequest?> UpdateAsync(UserVacancyRequest request)
        {
            var existingApplication = await _db.UserVacancyRequests.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingApplication != null)
            {
                _db.Entry(existingApplication).CurrentValues.SetValues(request);
                await _db.SaveChangesAsync();
                return request;
            }
            return null;
        }
    }
}
