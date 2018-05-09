using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tasks
{
    public class TaskQueue
    {
        public List<TaskBase> Tasks { get; private set; }
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

        public TaskQueue()
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
                NotifyTaskEvent(TaskEvent.NEW_TASK, newTask);
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
                Status = QueueStatus.IN_PROGRESS;
                NotifyTaskEvent(TaskEvent.START_TASK, task);
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
                Status = QueueStatus.WAITING;
                NotifyTaskEvent(TaskEvent.COMPLETE_TASK, task);
                return true;
            }

            return false;
        }

        public void CancelTask(TaskBase taskToCancel)
        {
            if (Tasks.Exists(t => t.Equals(taskToCancel)))
            {
                taskToCancel.OnCancel();
                Status = QueueStatus.WAITING;
                NotifyTaskEvent(TaskEvent.CANCEL_TASK, taskToCancel);
                Tasks.Remove(taskToCancel);
            }
        }

        public void RegisterHandler(TaskEvent taskEvent, TaskEventHandler handler)
        {
            if (!IsEventHandlerRegistered(taskEvent, handler))
            {
                if (!m_taskEventHandlers.ContainsKey(taskEvent))
                    m_taskEventHandlers.Add(taskEvent, handler);
                else
                    m_taskEventHandlers[taskEvent] += handler;
            }
        }

        public void UnregisterHandler(TaskEvent taskEvent, TaskEventHandler handler)
        {
            m_taskEventHandlers[taskEvent] -= handler;
        }

        public bool IsEventHandlerRegistered(TaskEvent taskEvent, TaskEventHandler handler)
        {
            if (m_taskEventHandlers.ContainsKey(taskEvent) && m_taskEventHandlers[taskEvent] != null)
                foreach (TaskEventHandler h in m_taskEventHandlers[taskEvent].GetInvocationList())
                    if (h == handler)
                        return true;
            return false;
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
            START_TASK,
            COMPLETE_TASK,
            CANCEL_TASK
        }
    }
}