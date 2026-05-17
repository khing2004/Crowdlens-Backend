using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Crowdlens_backend.Models
{
    public class User : IdentityUser // Inherit from IdentityUser
    {
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string SelfDescription { get; set; } = "";
        public string Pronouns { get; set; } = "Prefer not to say";
        public string AvatarBase64 { get; set; } = "";
        public bool NotificationsEnabled { get; set; } = true;
        public bool LocationSharingEnabled { get; set; } = true;
    }
}