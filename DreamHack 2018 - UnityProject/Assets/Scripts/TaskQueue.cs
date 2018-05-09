using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class TaskQueue
    {
        private List<TaskBase> Tasks { get; set; }
        public QueueStatus Status { get; private set; }

        public delegate void TaskEventHandler(TaskBase task);
        private Dictionary<TaskEvent, TaskEventHandler> m_taskEventHandlers;

        public TaskBase CurrentTask
        {
            get { return Tasks.Count > 0 ? Tasks[0] : null; }
        }

        public bool HasTask
        {
            get { return Tasks.Count > 0; }
        }

        TaskQueue()
        {
            Status = QueueStatus.WAITING;
            Tasks = new List<TaskBase>();
            m_taskEventHandlers = new Dictionary<TaskEvent, TaskEventHandler>();
        }

        /// <summary>
        /// Adds a task to the back of the queue
        /// </summary>
        /// <param name="newTask">The task to add to the queue</param>
        public void AddTask(TaskBase newTask)
        {
            if (!Tasks.Exists(t => t.Equals(newTask)))
            {
                Tasks.Add(newTask);
            }
        }

        public bool StartTask()
        {
            if (Status == QueueStatus.IN_PROGRESS)
                return false;

            TaskBase task = CurrentTask;
            if (task != null)
            {
                task.OnStart();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Marks the current task as complete, and removes it from the queue.
        /// </summary>
        /// <returns>True, if the task was found.</returns>
        public bool CompleteTask()
        {
            if (Status != QueueStatus.IN_PROGRESS)
                return false;

            TaskBase task = CurrentTask;
            if (task != null)
            {
                task.OnComplete();
                Tasks.Remove(task);
                return true;
            }

            return false;
        }

        public void CancelTask(TaskBase taskToCancel)
        {
            if (Tasks.Exists(t => t.Equals(taskToCancel)))
            {
                taskToCancel.OnCancel();
                Tasks.Remove(taskToCancel);
            }
        }

        public void RegisterHandler(TaskEvent taskEvent, TaskEventHandler handler)
        {
            m_taskEventHandlers[taskEvent] += handler;
        }

        public void NotifyTaskEvent(TaskEvent taskEvent, TaskBase task)
        {
            if(m_taskEventHandlers.ContainsKey(taskEvent))
            {
                m_taskEventHandlers[taskEvent](task);
            }
        }

        public enum QueueStatus
        {
            WAITING,
            IN_PROGRESS
        }

        public enum TaskEvent
        {
            NEW_TASK,
            COMPLETE_TASK,
            CANCEL_TASK
        }
    }
}