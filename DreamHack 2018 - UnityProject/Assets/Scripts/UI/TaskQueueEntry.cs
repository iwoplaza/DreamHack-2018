using Game.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TaskQueueEntry : MonoBehaviour
    {
        [SerializeField] protected Text m_titleText;

        protected TaskQueuePanel m_panel;
        protected TaskBase m_task;

        public void Setup(TaskQueuePanel panel, TaskBase task)
        {
            m_panel = panel;
            m_task = task;

            m_titleText.text = task.DisplayName;
        }

        public void RemoveTask()
        {
            m_panel.TaskQueue.CancelTask(m_task);
        }

        public void MoveUp()
        {

        }

        public void MoveDown()
        {

        }
    }
}
