using ResumeBuilder.DTOs;
using ResumeBuilder.Models;

namespace ResumeBuilder.Services
{
    public interface IResumeService
    {
        Task<IEnumerable<Resume>> GetUserResumesAsync(Guid userId);
        Task<Resume> CreateResumeAsync(Guid userId, ResumeDto dto);
        Task<Resume?> UpdateResumeAsync(Guid resumeId, ResumeDto dto);
        Task<bool> DeleteResumeAsync(Guid resumeId);
        Task<Resume?> GetResumeByIdAsync(Guid resumeId, Guid userId);

    }
}
