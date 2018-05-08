using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class WorkerPanel : MonoBehaviour
    {
        [SerializeField] protected Text m_nameText;

        protected IFocusTarget m_focusTarget = null;

        public InputField addTaskTitle;

        public GameObject task;
        public GameObject tasksPanel;
        public Text taskTitle;

        void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {
            gameObject.SetActive(false);
            if (WorldController.Instance != null)
            {
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_GAIN, OnFocusGained);
                WorldController.Instance.MainState.Focus.RegisterEventHandler(Focus.EventType.FOCUS_LOSS, OnFocusLost);
            }
        }

        public void UpdateTaskTitle()
        {
            taskTitle.text = addTaskTitle.text;
        }

        public void AddTask(string title)
        {
            // This is only a test function
            GameObject go = Instantiate(task, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            taskTitle.text = title;
            addTaskTitle.text = "";
            go.transform.parent = tasksPanel.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if(tasksPanel.transform.childCount > 5)
            {
                //Destroy();
            }
        }

        void OnFocusGained(IFocusTarget focusTarget)
        {
            gameObject.SetActive(true);

            m_focusTarget = focusTarget;

            if (m_focusTarget != null)
            {
                m_nameText.text = m_focusTarget.DisplayName;
            }
            else
            {
                OnFocusLost(focusTarget);
            }
        }

        void OnFocusLost(IFocusTarget focusTarget)
        {
            gameObject.SetActive(false);
        }
    }
}