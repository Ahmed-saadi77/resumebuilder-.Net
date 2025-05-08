using ResumeBuilder.Data;
using ResumeBuilder.DTOs;
using ResumeBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeBuilder.Services
{
    public class ResumeService : IResumeService
    {
        private readonly AppDbContext _context;

        public ResumeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resume>> GetUserResumesAsync(Guid userId)
        {
            // Return resumes for the user without sections (removed the section include part)
            return await _context.Resumes
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Resume> CreateResumeAsync(Guid userId, ResumeDto dto)
        {
            var resume = new Resume
            {
                Title = dto.Title,
                FullName = dto.FullName,
                Summary = dto.Summary,
                Experience = dto.Experience,
                Education = dto.Education,
                Skills = dto.Skills,
                UserId = userId
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
            return resume;
        }
        public async Task<Resume?> GetResumeByIdAsync(Guid resumeId, Guid userId)
        {
            return await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);
        }


        public async Task<Resume> UpdateResumeAsync(Guid resumeId, ResumeDto dto)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null) return null;

            resume.Title = dto.Title;
            resume.FullName = dto.FullName;
            resume.Summary = dto.Summary;
            resume.Experience = dto.Experience;
            resume.Education = dto.Education;
            resume.Skills = dto.Skills;

            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task<bool> DeleteResumeAsync(Guid resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null) return false;

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
