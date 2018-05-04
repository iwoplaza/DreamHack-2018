using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class TaskQueue
    {
        private List<TaskBase> Tasks { get; set; }

        TaskQueue()
        {
            Tasks = new List<TaskBase>();
        }

        /// <summary>
        /// Adds a task to the back of the queue
        /// </summary>
        /// <param name="newTask">The task to add to the queue</param>
        public void AddTask(TaskBase newTask)
        {
            Tasks.Add(newTask);
        }

        /// <summary>
        /// Marks a task as complete, and removes it from the queue.
        /// </summary>
        /// <param name="taskToComplete">Which task you'd like to mark as complete in the queue.</param>
        /// <returns>True, if the task was found.</returns>
        public bool CompleteTask(TaskBase taskToComplete)
        {
            if (Tasks.Exists(x => x.Equals(taskToComplete)))
            {
                Tasks.Remove(taskToComplete);
                return true;
            }

            return false;
        }
    }
}