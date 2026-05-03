using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTech.Wallet.Interfaces;

namespace FinTech.Wallet.Services.Notifications
{
    /// <summary>
    /// Email Notification Service
    /// Single Responsibility: Only sends email notifications
    /// Open/Closed Principle: Easy to add more notification channels
    /// </summary>
    public class EmailNotificationService : INotificationService
    {
        public async Task SendAsync(Notification notification)
        {
            try
            {
                // Simulate sending email
                await Task.Delay(200);
                Console.WriteLine($"📧 Email sent to user {notification.UserId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email service error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// SMS Notification Service
    /// </summary>
    public class SMSNotificationService : INotificationService
    {
        public async Task SendAsync(Notification notification)
        {
            try
            {
                // Simulate sending SMS
                await Task.Delay(150);
                Console.WriteLine($"📱 SMS sent to user {notification.UserId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS service error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Push Notification Service
    /// </summary>
    public class PushNotificationService : INotificationService
    {
        public async Task SendAsync(Notification notification)
        {
            try
            {
                // Simulate sending push notification
                await Task.Delay(100);
                Console.WriteLine($"🔔 Push notification sent to user {notification.UserId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Push notification service error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Composite Notification Service
    /// Sends notifications through multiple channels
    /// Composite Pattern: Combines multiple notification services
    /// </summary>
    public class CompositeNotificationService : INotificationService
    {
        private readonly List<INotificationService> _services;

        public CompositeNotificationService(params INotificationService[] services)
        {
            _services = new List<INotificationService>(services);
        }

        public async Task SendAsync(Notification notification)
        {
            var tasks = new List<Task>();
            foreach (var service in _services)
            {
                tasks.Add(service.SendAsync(notification));
            }
            await Task.WhenAll(tasks);
        }
    }
}
