using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private Dictionary<string, Task> tasksById = new Dictionary<string, Task>();
        private Queue<Task> tasks = new Queue<Task>();

        public void AddTask(Task task)
        {
            tasksById.Add(task.Id, task);
            tasks.Enqueue(task);
        }

        public bool Contains(Task task) => tasksById.ContainsKey(task.Id);

        public void DeleteTask(string taskId)
        {
            var curTask = tasksById[taskId];

            if (curTask == null)
            {
                throw new ArgumentException();
            }

            tasksById.Remove(taskId);
        }

        public Task ExecuteTask()
        {
            var curTask = tasks.Dequeue();

            if (curTask == null)
            {
                throw new ArgumentException();
            }

            return curTask;
        }

        public IEnumerable<Task> GetAllTasksOrderedByEETThenByName() => tasksById.Values
            .OrderByDescending(t => t.EstimatedExecutionTime).ThenBy(t => t.Name.Length);

        public IEnumerable<Task> GetDomainTasks(string domain)
        {
            var domainTasks = tasksById.Values.Where(t => t.Domain == domain);

            if (!domainTasks.Any())
            {
                throw new ArgumentException();
            }

            return domainTasks;
        }

        public Task GetTask(string taskId)
        {
            if (!tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            return tasksById[taskId];
        }

        public IEnumerable<Task> GetTasksInEETRange(int lowerBound, int upperBound) => tasks.Where(t =>
            t.EstimatedExecutionTime >= lowerBound && t.EstimatedExecutionTime <= upperBound);

        public void RescheduleTask(string taskId)
        {
            if (!tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            tasks.Enqueue(tasksById[taskId]);
        }

        public int Size() => tasksById.Count;
    }
}
