using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLens.Data;
using Microsoft.AspNetCore.Mvc;

namespace Crowdlens_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrowdController : ControllerBase
    {
        private readonly CrowdLensDbContext _context; // what does this do? My initial idea of this line is using CrowdlensDb as the context or the reference but its done in private and cannot be modified

        public CrowdController(CrowdLensDbContext context)
        {
            _context = context;
        }

        

    }
}