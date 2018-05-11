using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FocusPanel : MonoBehaviour
    {
        [SerializeField] protected Text m_nameText;

        protected FocusPortrait m_focusPortrait;
        protected HealthBar m_healthBar;
        protected TaskQueuePanel m_tileQueuePanel;
        
        protected IFocusTarget m_focusTarget = null;

        void Awake()
        {
            m_focusPortrait = GetComponentInChildren<FocusPortrait>();
            m_healthBar = GetComponentInChildren<HealthBar>();
            m_tileQueuePanel = GetComponentInChildren<TaskQueuePanel>();
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

        void OnFocusGained(IFocusTarget focusTarget)
        {
            gameObject.SetActive(true);

            m_focusTarget = focusTarget;

            if (m_focusTarget != null)
            {
                m_nameText.text = m_focusTarget.DisplayName;
                m_focusPortrait.Populate(m_focusTarget);
                if (m_focusTarget.Health != null)
                {
                    m_healthBar.gameObject.SetActive(true);
                    m_healthBar.Bind(m_focusTarget.Health);
                }
                else
                {
                    m_healthBar.gameObject.SetActive(false);
                }

                if (m_focusTarget is Worker)
                {
                    Worker worker = m_focusTarget as Worker;
                    m_tileQueuePanel.gameObject.SetActive(true);
                    m_tileQueuePanel.Populate(worker.TaskQueue);
                }
                else
                    m_tileQueuePanel.gameObject.SetActive(false);
            }
            else
            {
                OnFocusLost(focusTarget);
            }
        }

        void OnFocusLost(IFocusTarget focusTarget)
        {
            gameObject.SetActive(false);
            m_healthBar.Unbind();
        }
    }
}