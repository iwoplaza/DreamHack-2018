using Game.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class TaskQueuePanel : MonoBehaviour
    {
        [SerializeField] protected GameObject m_tileEntryPrefab;
        [SerializeField] protected float m_taskEntryHeight;

        Dictionary<TaskBase, TaskQueueEntry> m_taskObjects;

        public TaskQueue TaskQueue { get; private set; }

        void Awake()
        {
            m_taskObjects = new Dictionary<TaskBase, TaskQueueEntry>();
        }

        // Use this for initialization
        void Start()
        {

        }

        public void Populate(TaskQueue taskQueue)
        {
            if(TaskQueue != null)
            {
                TaskQueue.UnregisterHandler(TaskQueue.TaskEvent.NEW_TASK, OnNewTask);
                TaskQueue.UnregisterHandler(TaskQueue.TaskEvent.COMPLETE_TASK, OnTaskComplete);
                TaskQueue.UnregisterHandler(TaskQueue.TaskEvent.CANCEL_TASK, OnTaskCancel);
            }

            TaskQueue = taskQueue;
            TaskQueue.RegisterHandler(TaskQueue.TaskEvent.NEW_TASK, OnNewTask);
            TaskQueue.RegisterHandler(TaskQueue.TaskEvent.COMPLETE_TASK, OnTaskComplete);
            TaskQueue.RegisterHandler(TaskQueue.TaskEvent.CANCEL_TASK, OnTaskCancel);

            foreach (TaskQueueEntry entry in m_taskObjects.Values)
            {
                Destroy(entry.gameObject);
            }
            m_taskObjects.Clear();

            foreach (TaskBase task in taskQueue.Tasks)
            {
                GameObject taskObject = Instantiate(m_tileEntryPrefab, transform);
                TaskQueueEntry entry = taskObject.GetComponent<TaskQueueEntry>();
                entry.Setup(this, task);
                m_taskObjects.Add(task, entry);
            }
            PositionTasks();
        }

        void PositionTasks()
        {
            float offset = 0;
            foreach (TaskQueueEntry entry in m_taskObjects.Values)
            {
                entry.gameObject.transform.position = new Vector3(0, offset, 0);
                offset += m_taskEntryHeight;
            }
        }

        public void RemoveTaskObjectFor(TaskBase task)
        {
            foreach (KeyValuePair<TaskBase, TaskQueueEntry> pair in m_taskObjects)
            {
                if (pair.Key.Equals(task))
                {
                    Destroy(pair.Value.gameObject);
                    m_taskObjects.Remove(pair.Key);
                    return;
                }
            }
            PositionTasks();
        }

        public void OnNewTask(TaskBase task)
        {
            GameObject taskObject = Instantiate(m_tileEntryPrefab, transform);
            TaskQueueEntry entry = taskObject.GetComponent<TaskQueueEntry>();
            entry.Setup(this, task);
            m_taskObjects.Add(task, entry);
            PositionTasks();
        }

        public void OnTaskComplete(TaskBase task)
        {
            RemoveTaskObjectFor(task);
        }

        public void OnTaskCancel(TaskBase task)
        {
            RemoveTaskObjectFor(task);
        }
    }
}