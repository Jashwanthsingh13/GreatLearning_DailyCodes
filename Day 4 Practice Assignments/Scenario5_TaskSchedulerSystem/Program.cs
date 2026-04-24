using System;

namespace Scenario5_TaskSchedulerSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display system info
            TaskSchedulerSystem.DisplaySystemInfo();

            // Create task scheduler
            TaskSchedulerSystem scheduler = new TaskSchedulerSystem();

            // Add tasks
            scheduler.AddTask(new Task("TSK001", "Database Backup", "Backup production database", 5, DateTime.Now.AddDays(1)));
            scheduler.AddTask(new Task("TSK002", "Code Review", "Review pull requests", 4, DateTime.Now.AddDays(2)));
            scheduler.AddTask(new Task("TSK003", "Update Documentation", "Update API documentation", 3, DateTime.Now.AddDays(3)));
            scheduler.AddTask(new Task("TSK004", "Server Maintenance", "Perform server updates", 5));
            scheduler.AddTask(new Task("TSK005", "Email Newsletter", "Send weekly newsletter", 2));
            scheduler.AddTask(new Task("TSK006", "Bug Fixes", "Fix reported issues", 4));
            scheduler.AddTask(new Task("TSK007", "Performance Optimization", "Optimize database queries", 3));

            // Try to add duplicate task
            scheduler.AddTask(new Task("TSK001", "Duplicate Task", "This should fail", 2));

            // Display all tasks
            scheduler.DisplayAllTasks();

            // Display tasks sorted by priority
            scheduler.DisplayByPriority();

            // Display task execution order
            scheduler.DisplayTaskExecutionOrder();

            // Execute tasks
            Console.WriteLine("--- Executing Tasks ---");
            scheduler.ExecuteNextTask();
            scheduler.ExecuteNextTask();
            scheduler.ExecuteNextTask();

            // Display undo history
            scheduler.DisplayUndoHistory();

            // Undo operations
            Console.WriteLine("\n--- Undo Operations ---");
            scheduler.UndoLastTask();
            scheduler.UndoLastTask();

            // Execute remaining tasks
            Console.WriteLine("\n--- Executing Remaining Tasks ---");
            scheduler.ExecuteAllTasks();

            // Update task status
            Console.WriteLine("--- Updating Task Status ---");
            scheduler.UpdateTaskStatus("TSK002", TaskStatus.Cancelled);
            scheduler.UpdateTaskStatus("TSK003", TaskStatus.InProgress);

            // Display all tasks after operations
            scheduler.DisplayAllTasks();

            // Display statistics
            scheduler.DisplayStatistics();

            // Remove a completed task
            Console.WriteLine("--- Removing Tasks ---");
            scheduler.RemoveTask("TSK005");
            scheduler.RemoveTask("TSK999"); // Non-existent task

            // Final statistics
            scheduler.DisplayStatistics();
        }
    }
}
