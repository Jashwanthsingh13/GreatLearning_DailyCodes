using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario5_TaskSchedulerSystem
{
    /// <summary>
    /// Interface for task scheduling operations
    /// </summary>
    public interface ITaskScheduler
    {
        void AddTask(Task task);
        void RemoveTask(string taskId);
        void ExecuteNextTask();
        void UndoLastTask();
    }

    /// <summary>
    /// TaskSchedulerSystem class implements ITaskScheduler
    /// Manages task scheduling and execution
    /// </summary>
    public class TaskSchedulerSystem : ITaskScheduler
    {
        // Constants
        private const string SYSTEM_NAME = "Task Scheduler System";
        private const int MAX_TASKS = 10000;
        private const int MIN_PRIORITY = 1;
        private const int MAX_PRIORITY = 5;

        // Collections
        private Queue<string> taskExecutionQueue;              // FIFO task execution order
        private Stack<string> undoStack;                       // Undo last executed task
        private List<string> allTasks;                         // All tasks
        private SortedDictionary<int, string> priorityTasks;   // Priority-based tasks (sorted)
        private HashSet<string> uniqueTaskIds;                 // Ensure no duplicate tasks
        private Dictionary<string, Task> taskDatabase;         // Store task details

        public TaskSchedulerSystem()
        {
            taskExecutionQueue = new Queue<string>();
            undoStack = new Stack<string>();
            allTasks = new List<string>();
            priorityTasks = new SortedDictionary<int, string>();
            uniqueTaskIds = new HashSet<string>();
            taskDatabase = new Dictionary<string, Task>();
        }

        // Static method to display system info
        public static void DisplaySystemInfo()
        {
            Console.WriteLine($"=== {SYSTEM_NAME} ===");
            Console.WriteLine($"Max Tasks: {MAX_TASKS}");
            Console.WriteLine($"Priority Range: {MIN_PRIORITY}-{MAX_PRIORITY}");
            Console.WriteLine();
        }

        /// <summary>
        /// Add a new task to the system
        /// </summary>
        public void AddTask(Task task)
        {
            // Validate task ID uniqueness
            if (!uniqueTaskIds.Add(task.TaskId))
            {
                Console.WriteLine($"✗ Task ID '{task.TaskId}' already exists (duplicate)");
                return;
            }

            // Validate task count
            if (allTasks.Count >= MAX_TASKS)
            {
                Console.WriteLine($"✗ Cannot add task. Maximum capacity ({MAX_TASKS}) reached.");
                uniqueTaskIds.Remove(task.TaskId);
                return;
            }

            // Validate priority
            if (task.Priority < MIN_PRIORITY || task.Priority > MAX_PRIORITY)
            {
                Console.WriteLine($"✗ Invalid priority. Must be between {MIN_PRIORITY} and {MAX_PRIORITY}");
                uniqueTaskIds.Remove(task.TaskId);
                return;
            }

            // Add to collections
            allTasks.Add(task.TaskId);
            taskDatabase[task.TaskId] = task;
            taskExecutionQueue.Enqueue(task.TaskId);

            // Add to priority queue with unique key
            int priorityKey = (MAX_PRIORITY - task.Priority) * 1000 + priorityTasks.Count;
            priorityTasks[priorityKey] = task.TaskId;

            Console.WriteLine($"✓ Task added: {task.TaskName} (Priority: {task.Priority})");
        }

        /// <summary>
        /// Remove a task from the system
        /// </summary>
        public void RemoveTask(string taskId)
        {
            if (uniqueTaskIds.Contains(taskId))
            {
                uniqueTaskIds.Remove(taskId);
                allTasks.Remove(taskId);
                if (taskDatabase.ContainsKey(taskId))
                {
                    taskDatabase.Remove(taskId);
                }
                Console.WriteLine($"✓ Task '{taskId}' removed");
            }
            else
            {
                Console.WriteLine($"✗ Task '{taskId}' not found");
            }
        }

        /// <summary>
        /// Execute next task in queue (FIFO)
        /// </summary>
        public void ExecuteNextTask()
        {
            if (taskExecutionQueue.Count > 0)
            {
                string taskId = taskExecutionQueue.Dequeue();
                if (taskDatabase.ContainsKey(taskId))
                {
                    Task task = taskDatabase[taskId];
                    task.Status = TaskStatus.InProgress;
                    undoStack.Push(taskId);
                    Console.WriteLine($"▶ Executing: {task.TaskName}");
                    task.Status = TaskStatus.Completed;
                    Console.WriteLine($"✓ Completed: {task.TaskName}");
                }
            }
            else
            {
                Console.WriteLine("✗ No tasks in execution queue");
            }
        }

        /// <summary>
        /// Execute all pending tasks
        /// </summary>
        public void ExecuteAllTasks()
        {
            Console.WriteLine("\n--- Executing All Tasks (FIFO) ---");
            int executedCount = 0;
            while (taskExecutionQueue.Count > 0)
            {
                string taskId = taskExecutionQueue.Dequeue();
                if (taskDatabase.ContainsKey(taskId))
                {
                    Task task = taskDatabase[taskId];
                    if (task.Status == TaskStatus.Pending)
                    {
                        task.Status = TaskStatus.InProgress;
                        undoStack.Push(taskId);
                        Console.WriteLine($"✓ Executed: {task.TaskName}");
                        task.Status = TaskStatus.Completed;
                        executedCount++;
                    }
                }
            }
            Console.WriteLine($"Total executed: {executedCount}\n");
        }

        /// <summary>
        /// Undo last executed task
        /// </summary>
        public void UndoLastTask()
        {
            if (undoStack.Count > 0)
            {
                string taskId = undoStack.Pop();
                if (taskDatabase.ContainsKey(taskId))
                {
                    Task task = taskDatabase[taskId];
                    task.Status = TaskStatus.Pending;
                    taskExecutionQueue.Enqueue(taskId);
                    Console.WriteLine($"↶ Undone: {task.TaskName} (Reset to Pending)");
                }
            }
            else
            {
                Console.WriteLine("✗ No tasks to undo");
            }
        }

        /// <summary>
        /// Update task status
        /// </summary>
        public void UpdateTaskStatus(string taskId, TaskStatus status)
        {
            if (taskDatabase.ContainsKey(taskId))
            {
                taskDatabase[taskId].Status = status;
                Console.WriteLine($"✓ Task '{taskId}' status updated to: {status}");
            }
            else
            {
                Console.WriteLine($"✗ Task '{taskId}' not found");
            }
        }

        /// <summary>
        /// Display all tasks in execution order (FIFO)
        /// </summary>
        public void DisplayTaskExecutionOrder()
        {
            Console.WriteLine("\n--- Tasks in Execution Order (FIFO Queue) ---");
            if (taskExecutionQueue.Count == 0)
            {
                Console.WriteLine("No tasks pending execution");
                return;
            }

            var tempQueue = new Queue<string>(taskExecutionQueue);
            int count = 1;
            while (tempQueue.Count > 0)
            {
                string taskId = tempQueue.Dequeue();
                Task task = taskDatabase[taskId];
                Console.WriteLine($"{count}. {task}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display tasks sorted by priority
        /// </summary>
        public void DisplayByPriority()
        {
            Console.WriteLine("\n--- Tasks Sorted by Priority (SortedDictionary) ---");
            if (priorityTasks.Count == 0)
            {
                Console.WriteLine("No tasks available");
                return;
            }

            var seen = new HashSet<string>();
            foreach (var taskId in priorityTasks.Values)
            {
                if (seen.Add(taskId) && taskDatabase.ContainsKey(taskId))
                {
                    Task task = taskDatabase[taskId];
                    Console.WriteLine($"Priority {task.Priority}: {task.TaskName} - {task.Description}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all tasks
        /// </summary>
        public void DisplayAllTasks()
        {
            Console.WriteLine("\n--- All Tasks (List) ---");
            if (allTasks.Count == 0)
            {
                Console.WriteLine("No tasks available");
                return;
            }

            int count = 1;
            foreach (var taskId in allTasks)
            {
                if (taskDatabase.ContainsKey(taskId))
                {
                    Console.WriteLine($"{count}. {taskDatabase[taskId]}");
                    count++;
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display undo history
        /// </summary>
        public void DisplayUndoHistory()
        {
            Console.WriteLine("\n--- Undo History (LIFO Stack) ---");
            if (undoStack.Count == 0)
            {
                Console.WriteLine("No tasks in undo history");
                return;
            }

            var tempStack = new Stack<string>(undoStack);
            int count = 1;
            while (tempStack.Count > 0)
            {
                string taskId = tempStack.Pop();
                if (taskDatabase.ContainsKey(taskId))
                {
                    Console.WriteLine($"{count}. {taskDatabase[taskId].TaskName}");
                    count++;
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display system statistics
        /// </summary>
        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== Task Scheduler Statistics ===");
            Console.WriteLine($"Total Tasks: {allTasks.Count}");
            Console.WriteLine($"Unique Task IDs: {uniqueTaskIds.Count}");
            Console.WriteLine($"Pending Tasks: {taskExecutionQueue.Count}");
            Console.WriteLine($"Undo History Length: {undoStack.Count}");
            
            int completed = allTasks.Count(tid => taskDatabase.ContainsKey(tid) && taskDatabase[tid].Status == TaskStatus.Completed);
            int inProgress = allTasks.Count(tid => taskDatabase.ContainsKey(tid) && taskDatabase[tid].Status == TaskStatus.InProgress);
            Console.WriteLine($"Completed Tasks: {completed}");
            Console.WriteLine($"In Progress: {inProgress}");
            Console.WriteLine();
        }

        public int GetTaskCount() => allTasks.Count;
    }
}
