using System;

namespace Scenario5_TaskSchedulerSystem
{
    /// <summary>
    /// Task class represents a scheduled task
    /// </summary>
    public class Task
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; } // 1-5, higher is more important
        public TaskStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public Task(string taskId, string taskName, string description, int priority, DateTime? dueDate = null)
        {
            TaskId = taskId;
            TaskName = taskName;
            Description = description;
            Priority = priority;
            Status = TaskStatus.Pending;
            CreatedDate = DateTime.Now;
            DueDate = dueDate;
        }

        public override string ToString()
        {
            string dueStr = DueDate.HasValue ? $", Due: {DueDate:dd/MM/yyyy}" : "";
            return $"[{TaskId}] {TaskName} - {Description} (Priority: {Priority}/5, Status: {Status}){dueStr}";
        }
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
}
