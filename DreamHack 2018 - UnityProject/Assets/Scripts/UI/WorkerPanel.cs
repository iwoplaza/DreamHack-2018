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

        // Update is called once per frame
        void Update()
        {

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