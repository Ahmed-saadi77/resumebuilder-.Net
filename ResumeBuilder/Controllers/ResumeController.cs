using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.DTOs;
using ResumeBuilder.Services;
using System.Security.Claims;

namespace ResumeBuilder.Controllers
{
    [ApiController]
    [Route("api/resumes")]
    [Authorize] // Ensure authentication is required
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        // Extract UserID from the JWT token
        private Guid GetUserIdFromToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userId!); // Make sure the token contains NameIdentifier
        }

        // Get all resumes for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetUserResumes()
        {
            var userId = GetUserIdFromToken();
            var resumes = await _resumeService.GetUserResumesAsync(userId);
            return Ok(resumes);
        }

        // Create a new resume for the logged-in user
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] ResumeDto dto)
        {
            var userId = GetUserIdFromToken();
            var resume = await _resumeService.CreateResumeAsync(userId, dto);
            return Ok(resume);
        }

        // Update a resume for the logged-in user
        [HttpPut("{resumeId}")]
        public async Task<IActionResult> UpdateResume(Guid resumeId, [FromBody] ResumeDto dto)
        {
            var userId = GetUserIdFromToken();
            var updated = await _resumeService.UpdateResumeAsync(resumeId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // Get a single resume by ID for the logged-in user
        [HttpGet("{resumeId}")]
        public async Task<IActionResult> GetResumeById(Guid resumeId)
        {
            var userId = GetUserIdFromToken();
            var resume = await _resumeService.GetResumeByIdAsync(resumeId, userId);
            if (resume == null) return NotFound();
            return Ok(resume);
        }


        // Delete a resume for the logged-in user
        [HttpDelete("{resumeId}")]
        public async Task<IActionResult> DeleteResume(Guid resumeId)
        {
            var result = await _resumeService.DeleteResumeAsync(resumeId);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
