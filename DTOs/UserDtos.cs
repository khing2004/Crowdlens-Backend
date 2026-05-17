namespace Crowdlens_backend.DTOs
{
    public class UserProfileDto
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Pronouns { get; set; } = "";
        public string Address { get; set; } = "";
        public string Birthday { get; set; } = "";   // "yyyy-MM-dd" or ""
        public string Bio { get; set; } = "";
        public string Avatar { get; set; } = "";      // base64 data URL or ""
    }

    public class UserSettingsDto
    {
        public bool NotificationsEnabled { get; set; } = true;
        public bool LocationSharingEnabled { get; set; } = true;
    }

    public class UserKarmaDto
    {
        public int Karma { get; set; }
    }
}
