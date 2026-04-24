using System;

namespace Scenario2_SocialMediaPlatform
{
    /// <summary>
    /// User class represents a user in the social media platform
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public User(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"UserId: {UserId}, Username: {Username}, Email: {Email}, Joined: {CreatedDate:dd/MM/yyyy}";
        }
    }
}
