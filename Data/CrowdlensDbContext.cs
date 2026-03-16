using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Crowdlens_backend.Models;

namespace CrowdLens.Data;


public class CrowdLensDbContext : IdentityDbContext<User>
{
    public CrowdLensDbContext(DbContextOptions<CrowdLensDbContext> options) 
        : base(options) { }
}