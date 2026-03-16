using Microsoft.AspNetCore.Identity;

namespace CrowdLens.Services;

public class EmailSender : IEmailSender<IdentityUser>
{
    // This is the method triggered by /forgotPassword
    public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
    {
        Console.WriteLine("**************************************************");
        Console.WriteLine($"FORGOT PASSWORD CODE FOR: {email}");
        Console.WriteLine($"YOUR RESET CODE IS: {resetCode}");
        Console.WriteLine("**************************************************");
        
        return Task.CompletedTask;
    }

    // These are required by the interface but not used for forgotPassword
    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink) => Task.CompletedTask;
    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink) => Task.CompletedTask;
}