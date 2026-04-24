using System;

namespace Scenario2_SocialMediaPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display platform info
            SocialMediaPlatform.DisplayPlatformInfo();

            // Create social media platform
            SocialMediaPlatform platform = new SocialMediaPlatform();

            // Register users
            platform.RegisterUser(new User(1, "Alice", "alice@example.com"));
            platform.RegisterUser(new User(2, "Bob", "bob@example.com"));
            platform.RegisterUser(new User(3, "Charlie", "charlie@example.com"));
            platform.RegisterUser(new User(4, "Diana", "diana@example.com"));

            // Display registered users
            platform.DisplayAllUsers();

            // Add posts
            platform.AddPost("Just launched my new project! #excited");
            platform.AddPost("Beautiful sunset today! #nature");
            platform.AddPost("Learning C# collections is fun! #coding");
            platform.AddPost("Coffee and code - the perfect combination ☕");

            // Display posts
            platform.DisplayAllPosts();

            // Like posts
            platform.LikePost("Just launched my new project! #excited");
            platform.LikePost("Just launched my new project! #excited");
            platform.LikePost("Beautiful sunset today! #nature");
            platform.LikePost("Learning C# collections is fun! #coding");
            platform.LikePost("Learning C# collections is fun! #coding");
            platform.LikePost("Learning C# collections is fun! #coding");

            // Display posts with likes
            platform.DisplayAllPosts();

            // Display recent actions
            platform.DisplayRecentActions();

            // Undo last action
            Console.WriteLine("\n--- Undo Operations ---");
            platform.UndoLastAction();
            platform.UndoLastAction();

            // Remove a post
            platform.RemovePost("Coffee and code - the perfect combination ☕");

            // Display final posts
            platform.DisplayAllPosts();

            // Process notifications
            platform.ProcessNotifications();

            // Display statistics
            platform.DisplayStatistics();
        }
    }
}
