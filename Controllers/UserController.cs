using System.Security.Claims;
using CrowdLens.Data;
using Crowdlens_backend.DTOs;
using Crowdlens_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crowdlens_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly CrowdLensDbContext _context;

        public UserController(UserManager<User> userManager, CrowdLensDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ── GET /api/User/profile ─────────────────────────────────────────────
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var birthday = user.BirthDate == default
                ? ""
                : user.BirthDate.ToString("yyyy-MM-dd");

            return Ok(new UserProfileDto
            {
                Username = user.FullName,
                Email    = user.Email ?? "",
                Pronouns = user.Pronouns,
                Address  = user.Address,
                Birthday = birthday,
                Bio      = user.SelfDescription,
                Avatar   = user.AvatarBase64,
            });
        }

        // ── PUT /api/User/profile ─────────────────────────────────────────────
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.FullName        = dto.Username.Trim();
            user.SelfDescription = dto.Bio.Trim();
            user.Pronouns        = dto.Pronouns;
            user.Address         = dto.Address.Trim();
            user.AvatarBase64    = dto.Avatar;

            if (!string.IsNullOrWhiteSpace(dto.Birthday) &&
                DateOnly.TryParse(dto.Birthday, out var date))
            {
                user.BirthDate = date.ToDateTime(TimeOnly.MinValue);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile updated." });
        }

        // ── GET /api/User/karma ───────────────────────────────────────────────
        [HttpGet("karma")]
        public async Task<IActionResult> GetKarma()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var karma = await _context.Reports
                .Where(r => r.UserId == userId)
                .SelectMany(r => r.Votes)
                .SumAsync(v => v.VoteType == "Up" ? 1 : -1);

            return Ok(new UserKarmaDto { Karma = karma });
        }

        // ── GET /api/User/settings ────────────────────────────────────────────
        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(new UserSettingsDto
            {
                NotificationsEnabled  = user.NotificationsEnabled,
                LocationSharingEnabled = user.LocationSharingEnabled,
            });
        }

        // ── PUT /api/User/settings ────────────────────────────────────────────
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UserSettingsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.NotificationsEnabled  = dto.NotificationsEnabled;
            user.LocationSharingEnabled = dto.LocationSharingEnabled;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Settings updated." });
        }
    }
}
