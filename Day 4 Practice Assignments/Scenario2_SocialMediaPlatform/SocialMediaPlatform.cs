using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario2_SocialMediaPlatform
{
    /// <summary>
    /// Interface for social media engagement operations
    /// </summary>
    public interface IEngagementSystem
    {
        void AddPost(string post);
        void LikePost(string post);
        void RemovePost(string post);
        void AddRecentAction(string action);
    }

    /// <summary>
    /// SocialMediaPlatform class implements IEngagementSystem
    /// Manages users, posts, likes, and user actions
    /// </summary>
    public class SocialMediaPlatform : IEngagementSystem
    {
        // Constants
        private const string PLATFORM_NAME = "Social Media Platform";
        private const int MAX_USERS = 10000;
        private const int MAX_POSTS = 100000;

        // Collections
        private List<string> posts;                             // Store all posts
        private Dictionary<string, int> postLikes;             // Track likes per post
        private HashSet<int> uniqueUserIds;                    // Track unique user IDs
        private Stack<string> recentActions;                   // Track recent actions (undo functionality)
        private Queue<string> notificationQueue;               // Process notifications (FIFO)
        private Dictionary<int, User> users;                   // Dictionary to store users

        public SocialMediaPlatform()
        {
            posts = new List<string>();
            postLikes = new Dictionary<string, int>();
            uniqueUserIds = new HashSet<int>();
            recentActions = new Stack<string>();
            notificationQueue = new Queue<string>();
            users = new Dictionary<int, User>();
        }

        // Static method to display platform info
        public static void DisplayPlatformInfo()
        {
            Console.WriteLine($"=== {PLATFORM_NAME} ===");
            Console.WriteLine($"Max Users: {MAX_USERS}");
            Console.WriteLine($"Max Posts: {MAX_POSTS}");
            Console.WriteLine();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public void RegisterUser(User user)
        {
            if (uniqueUserIds.Count < MAX_USERS)
            {
                if (uniqueUserIds.Add(user.UserId))
                {
                    users.Add(user.UserId, user);
                    recentActions.Push($"User {user.Username} registered");
                    notificationQueue.Enqueue($"Welcome to {PLATFORM_NAME}, {user.Username}!");
                    Console.WriteLine($"✓ User registered: {user.Username}");
                }
                else
                {
                    Console.WriteLine($"✗ User ID {user.UserId} already exists");
                }
            }
            else
            {
                Console.WriteLine($"✗ Cannot register user. Maximum capacity ({MAX_USERS}) reached.");
            }
        }

        /// <summary>
        /// Add a new post
        /// </summary>
        public void AddPost(string post)
        {
            if (posts.Count < MAX_POSTS)
            {
                posts.Add(post);
                postLikes[post] = 0; // Initialize likes to 0
                recentActions.Push($"Post added: '{post}'");
                notificationQueue.Enqueue($"New post: {post}");
                Console.WriteLine($"✓ Post added: '{post}'");
            }
            else
            {
                Console.WriteLine($"✗ Cannot add post. Maximum capacity ({MAX_POSTS}) reached.");
            }
        }

        /// <summary>
        /// Like a post
        /// </summary>
        public void LikePost(string post)
        {
            if (postLikes.ContainsKey(post))
            {
                postLikes[post]++;
                recentActions.Push($"Post liked: '{post}' (Total likes: {postLikes[post]})");
                Console.WriteLine($"✓ Liked: '{post}' (Total likes: {postLikes[post]})");
            }
            else
            {
                Console.WriteLine($"✗ Post not found: '{post}'");
            }
        }

        /// <summary>
        /// Remove a post
        /// </summary>
        public void RemovePost(string post)
        {
            if (posts.Contains(post))
            {
                posts.Remove(post);
                postLikes.Remove(post);
                recentActions.Push($"Post removed: '{post}'");
                notificationQueue.Enqueue($"Post deleted: {post}");
                Console.WriteLine($"✓ Post removed: '{post}'");
            }
            else
            {
                Console.WriteLine($"✗ Post not found: '{post}'");
            }
        }

        /// <summary>
        /// Add a recent action (for tracking)
        /// </summary>
        public void AddRecentAction(string action)
        {
            recentActions.Push(action);
        }

        /// <summary>
        /// Undo last action/post
        /// </summary>
        public void UndoLastAction()
        {
            if (recentActions.Count > 0)
            {
                string lastAction = recentActions.Pop();
                Console.WriteLine($"✓ Undo: {lastAction}");
            }
            else
            {
                Console.WriteLine("✗ No actions to undo");
            }
        }

        /// <summary>
        /// Process all notifications (FIFO)
        /// </summary>
        public void ProcessNotifications()
        {
            Console.WriteLine("\n--- Processing Notifications (FIFO) ---");
            if (notificationQueue.Count == 0)
            {
                Console.WriteLine("No notifications to process");
                return;
            }

            int count = 1;
            while (notificationQueue.Count > 0)
            {
                string notification = notificationQueue.Dequeue();
                Console.WriteLine($"{count}. {notification}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all posts with likes
        /// </summary>
        public void DisplayAllPosts()
        {
            Console.WriteLine("\n--- All Posts ---");
            if (posts.Count == 0)
            {
                Console.WriteLine("No posts available");
                return;
            }

            for (int i = 0; i < posts.Count; i++)
            {
                string post = posts[i];
                Console.WriteLine($"{i + 1}. \"{post}\" - {postLikes[post]} likes");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all registered users
        /// </summary>
        public void DisplayAllUsers()
        {
            Console.WriteLine("\n--- All Users ---");
            if (users.Count == 0)
            {
                Console.WriteLine("No users registered");
                return;
            }

            foreach (var user in users.Values)
            {
                Console.WriteLine(user);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display recent actions (undo stack)
        /// </summary>
        public void DisplayRecentActions()
        {
            Console.WriteLine("\n--- Recent Actions (LIFO) ---");
            if (recentActions.Count == 0)
            {
                Console.WriteLine("No recent actions");
                return;
            }

            var tempStack = new Stack<string>(recentActions);
            int count = 1;
            while (tempStack.Count > 0)
            {
                Console.WriteLine($"{count}. {tempStack.Pop()}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Get statistics
        /// </summary>
        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== Platform Statistics ===");
            Console.WriteLine($"Total Users: {uniqueUserIds.Count}");
            Console.WriteLine($"Total Posts: {posts.Count}");
            Console.WriteLine($"Total Likes: {postLikes.Values.Sum()}");
            Console.WriteLine($"Pending Notifications: {notificationQueue.Count}");
            Console.WriteLine();
        }

        public int GetUserCount() => uniqueUserIds.Count;
        public int GetPostCount() => posts.Count;
        public int GetTotalLikes() => postLikes.Values.Sum();
    }
}
