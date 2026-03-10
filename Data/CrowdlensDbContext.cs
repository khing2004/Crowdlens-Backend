using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CrowdLens.Data;


public class CrowdLensDbContext : IdentityDbContext<IdentityUser>
{
    public CrowdLensDbContext(DbContextOptions<CrowdLensDbContext> options) : base(options) { }
}